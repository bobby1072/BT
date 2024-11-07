using BT.Common.FastArray.Proto;
using BT.Common.Helpers.TypeFor;
using BT.Common.OperationTimer.Proto;
using BT.Common.WorkflowActivities.Abstract;
using BT.Common.WorkflowActivities.Activities.Abstract;
using BT.Common.WorkflowActivities.Activities.Concrete;
using BT.Common.WorkflowActivities.Completed;
using BT.Common.WorkflowActivities.Contexts;
using BT.Common.WorkflowActivities.Exceptions;
using BT.Common.WorkflowActivities.Services.Abstract;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BT.Common.WorkflowActivities.Services.Concrete
{
    internal class WorkflowExecuterService
     : IWorkflowExecuterService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<WorkflowExecuterService> _logger;
        public WorkflowExecuterService(
            IServiceProvider serviceProvider,
            ILogger<WorkflowExecuterService> logger
        )
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task<CompletedWorkflow<TContext,TInputContext, TOutputContext, TReturn>> ExecuteAsync<TContext, TInputContext, TOutputContext, TReturn>(
            TypeFor<IWorkflow<TContext, TInputContext, TOutputContext, TReturn>> workflowToExecute
           )
            where TContext : WorkflowContext<
                    TInputContext,
                    TOutputContext,
                    TReturn
                >
            where TInputContext : WorkflowInputContext
            where TOutputContext : WorkflowOutputContext<TReturn>
        {
            var workflowStartTime = DateTime.UtcNow;
            var (timeTaken, (executedActivityBlocks, foundWorkflow)) = await OperationTimerUtils.TimeWithResultsAsync(() => ExecuteInnerAsync(workflowToExecute));
            var completedWorkflow = new CompletedWorkflow<TContext, TInputContext, TOutputContext, TReturn>{ActualWorkflow = foundWorkflow, StartedAt = workflowStartTime, CompletedAt = DateTime.UtcNow, TotalTimeTaken = timeTaken, CompletedActivities = executedActivityBlocks };
            _logger.LogInformation("----------   Workflow finished: {SerialisedWorkflow}   ----------", JsonSerializer.Serialize(completedWorkflow));

            return completedWorkflow;
        }
        private async Task<(IReadOnlyCollection<CompletedActivityBlockToRun<object?, object?>>, IWorkflow<TContext, TInputContext, TOutputContext, TReturn>)> ExecuteInnerAsync<TContext, TInputContext, TOutputContext, TReturn>(
            TypeFor<IWorkflow<TContext, TInputContext, TOutputContext, TReturn>> workflowToExecute
        )
            where TContext : WorkflowContext<
                    TInputContext,
                    TOutputContext,
                    TReturn
                >
            where TInputContext : WorkflowInputContext
            where TOutputContext : WorkflowOutputContext<TReturn>
        {

            var foundWorkflow = _serviceProvider.GetService(workflowToExecute.ActualType) as IWorkflow<TContext, TInputContext, TOutputContext, TReturn> ?? throw new WorkflowException(WorkflowConstants.CouldNotResolveActivity);
            var completedActivityBlockList = new List<CompletedActivityBlockToRun<object?, object?>>();

            try
            {
                _logger.LogInformation("----------  Entering workflow execution: {WorkflowName} {WorkflowId}  ----------", foundWorkflow.Name, foundWorkflow.WorkflowRunId);

                await foundWorkflow.PreWorkflowRoutine();

                var allActivityBlocksToRun = foundWorkflow.ActivitiesToRun.FastArraySelect(x => (x.ExecutionType, x.ActivitesToRun.FastArraySelect(x => ToActualActivityToRun(x))));

                foreach (var singleActivityBlock in allActivityBlocksToRun)
                {
                    var workflowActivityList = new List<CompletedWorkflowActivity<object?, object?>>();
                    var (exeType, funcsAndActivities) = singleActivityBlock;
                    var funcAndActivityCount = funcsAndActivities.Count();
                    if (funcAndActivityCount == 0)
                    {
                        continue;
                    }

                    if (exeType == ActivityBlockExecutionTypeEnum.Sync)
                    {
                        foreach (var funcAndActivity in funcsAndActivities)
                        {
                            var (singleFuncAndActualActivity, singleActivity) = funcAndActivity;
                            var (timeTakenForActivity, (activityResult, timesRetried)) = OperationTimerUtils.TimeWithResults(singleFuncAndActualActivity);
                            workflowActivityList.Add(new CompletedWorkflowActivity<object?, object?>{Activity = singleActivity, NumberOfRetriesTaken= timesRetried, TotalTimeTaken =timeTakenForActivity, ActivityResult = activityResult });
                        }
                    }
                    else if (exeType == ActivityBlockExecutionTypeEnum.Async)
                    {
                        if (funcAndActivityCount == 1)
                        {
                            var (singleFuncAndActualActivity, singleActivity) = funcsAndActivities.FirstOrDefault()!;
                            var (timeTakenForActivity, (activityResult, timesRetried)) = await OperationTimerUtils.TimeWithResultsAsync(singleFuncAndActualActivity);
                            workflowActivityList.Add(new CompletedWorkflowActivity<object?, object?> { Activity = singleActivity, NumberOfRetriesTaken = timesRetried, TotalTimeTaken = timeTakenForActivity, ActivityResult = activityResult });
                            continue;
                        }
                        else
                        {
                            var tasks = funcsAndActivities.FastArraySelect(async x =>
                            {
                                var (singleFuncAndActualActivity, singleActivity) = x;
                                var (timeTakenForActivity, (activityResult, timesRetried)) = await OperationTimerUtils.TimeWithResultsAsync(singleFuncAndActualActivity);
                                return new CompletedWorkflowActivity<object?, object?> { Activity = singleActivity, NumberOfRetriesTaken = timesRetried, TotalTimeTaken = timeTakenForActivity, ActivityResult = activityResult };
                            });

                            var completedActivities = await Task.WhenAll(tasks);
                            workflowActivityList.AddRange(completedActivities);
                        }
                    }

                    completedActivityBlockList.Add(new CompletedActivityBlockToRun<object?, object?>{ CompletedWorkflowActivities = workflowActivityList, ExecutionType = exeType });
                }
            }
            catch (WorkflowException e)
            {
                _logger.LogError(e, "Uncaught workflow exception occurred during execution of workflow: {WorkflowName} {WorkflowId}", foundWorkflow.Name, foundWorkflow.WorkflowRunId);
                _logger.LogInformation("----------Exiting workflow execution: {WorkflowName} {WorkflowId}-----------", foundWorkflow.Name, foundWorkflow.WorkflowRunId);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Uncaught exception occurred during execution of workflow: {WorkflowName} {WorkflowId}", foundWorkflow.Name, foundWorkflow.WorkflowRunId);
            }
            finally
            {
                _logger.LogInformation("----------Exiting workflow execution: {WorkflowName} {WorkflowId}-----------", foundWorkflow.Name, foundWorkflow.WorkflowRunId);
            }


            var finalActivityResult = completedActivityBlockList.LastOrDefault()?.CompletedWorkflowActivities.LastOrDefault()?.ActivityResult;
            if (finalActivityResult == ActivityResultEnum.Success)
            {
                await foundWorkflow.PostSuccessfulWorkflowRoutine();
            }
            else if (finalActivityResult == ActivityResultEnum.Fail)
            {
                await foundWorkflow.PostUnsuccessfulWorkflowRoutine();
            }

            return (completedActivityBlockList, foundWorkflow);
        }










        private
             (Func<
            Task<(ActivityResultEnum ActivityResult, int TimesRetried)>
        >, IActivity<TActivityContextItem?, TActivityReturnItem?>)
         ToActualActivityToRun<TActivityContextItem, TActivityReturnItem>(
            ActivityToRun<TActivityContextItem, TActivityReturnItem> activity
        )
        {
            var resolvedActivity =
                _serviceProvider.GetService(activity.ActivityType.ActualType)
                as IActivity<TActivityContextItem?, TActivityReturnItem?>
             ?? throw new WorkflowException(WorkflowConstants.CouldNotResolveActivity);


            return (() => ActualFunc(activity, resolvedActivity), resolvedActivity);
        }

        private async Task<(
            ActivityResultEnum ActivityResult,
            int TimesRetried
        )> ActualFunc<TActivityContextItem, TActivityReturnItem>(
            ActivityToRun<TActivityContextItem, TActivityReturnItem> activity,
            IActivity<TActivityContextItem?, TActivityReturnItem?> resolvedActivity
        )
        {
            var timesToRetry =
                activity.OverrideRetryCount ?? activity.DefaultRetryAttribute?.RetryCount ?? 1;

            var secondsBetweenRetries =
                activity.OverrideSecondsBetweenRetries
                ?? activity.DefaultRetryAttribute?.SecondsBetweenRetries
                ?? 0;

            var retryOnException =
                activity.OverrideRetryOnException
                ?? activity.DefaultRetryAttribute?.RetryOnException
                ?? true;


            for (int retryCounter = 0; retryCounter < timesToRetry; retryCounter++)
            {
                try
                {
                    if (retryCounter > 0 && secondsBetweenRetries > 0)
                    {
                        await Task.Delay(secondsBetweenRetries * 1000);
                    }
                    _logger.LogInformation("Attempting to execute {ActivityName}, {ActivityRunId}. On attempt: {AttemptNumber}", resolvedActivity.Name, resolvedActivity.ActivityRunId, retryCounter + 1);


                    Func<TActivityContextItem?, Task<(ActivityResultEnum ActivityResult, TActivityReturnItem? ActualResult)>> mainResultFunc = resolvedActivity.ExecuteAsync;

                    if (activity.ActivityWrapperFunc is not null)
                    {
                        mainResultFunc = (x) => activity.ActivityWrapperFunc.Invoke(x, mainResultFunc);
                    }

                    var (timeTakenForAttempt, (ActivityResult, ActualResult)) = await OperationTimerUtils.TimeWithResultsAsync(() => mainResultFunc.Invoke(activity.ContextItem));


                    if (ActivityResult == ActivityResultEnum.Success)
                    {
                        _logger.LogInformation("Activity {ActivityName}, {ActivityRunId} executed successfully and took {TimeTaken}ms . On attempt: {AttemptNumber}", resolvedActivity.Name, resolvedActivity.ActivityRunId, timeTakenForAttempt.Milliseconds, retryCounter + 1);
                        return (ActivityResult, retryCounter + 1);
                    }
                    _logger.LogWarning("Activity {ActivityName}, {ActivityRunId} failed and took {TimeTaken}ms . On attempt: {AttemptNumber}", resolvedActivity.Name, resolvedActivity.ActivityRunId, timeTakenForAttempt.Milliseconds, retryCounter + 1);
                    if (retryCounter == timesToRetry - 1)
                    {
                        return (ActivityResult, retryCounter + 1);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Uncaught exception with message {ExceptionMessage} in activity: {ActivityName}, {ActivityRunId}. On attempt: {AttemptNumber}", ex.Message, resolvedActivity.Name, resolvedActivity.ActivityRunId, retryCounter + 1);
                }
            }

            return (ActivityResultEnum.Fail, timesToRetry);
        }
    }
}