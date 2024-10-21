using BT.Common.FastArray.Proto;
using BT.Common.Helpers.TypeFor;
using BT.Common.OperationTimer.Proto;
using BT.Common.Workflow.Abstract;
using BT.Common.Workflow.Activities.Abstract;
using BT.Common.Workflow.Activities.Concrete;
using BT.Common.Workflow.Completed;
using BT.Common.Workflow.Contexts;
using BT.Common.Workflow.Exceptions;
using BT.Common.Workflow.Services.Abstract;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BT.Common.Workflow.Services.Concrete
{
    public class WorkflowExecuterService
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

        public Task<CompletedWorkflow<TContext, TReturn>> ExecuteAsync<TContext, TReturn>(
            TypeFor<IWorkflow<TContext, TReturn>> workflowToExecute
           )
    where TContext : IWorkflowContext<
            IWorkflowInputContext,
            IWorkflowOutputContext<TReturn>,
            TReturn
        >
        {
            var foundWorkflow = (_serviceProvider.GetService(workflowToExecute.ActualType) as IWorkflow<TContext, TReturn>) ?? throw new WorkflowException(WorkflowConstants.CouldNotResolveActivity);

            return ExecuteAsync(foundWorkflow);
        }

        public async Task<CompletedWorkflow<TContext, TReturn>> ExecuteAsync<TContext, TReturn>(
            IWorkflow<TContext, TReturn> workflowToExecute
        )
            where TContext : IWorkflowContext<
                    IWorkflowInputContext,
                    IWorkflowOutputContext<TReturn>,
                    TReturn
                >
        {
            var workflowStartTime = DateTime.UtcNow;

            var (timeTaken,executedActivityBlocks) = await OperationTimerUtils.TimeWithResultsAsync(() => ExecuteAsyncInner(workflowToExecute));

            var completedWorkflow = new CompletedWorkflow<TContext, TReturn>(workflowToExecute, workflowStartTime, DateTime.UtcNow, timeTaken, executedActivityBlocks);

            
            _logger.LogInformation("----------   Workflow finished: {SerialisedWorkflow}   ----------", JsonSerializer.Serialize(completedWorkflow));

            return completedWorkflow;
        }
        private async Task<IReadOnlyCollection<CompletedActivityBlockToRun<object?, object?>>> ExecuteAsyncInner<TContext, TReturn>(
            IWorkflow<TContext, TReturn> workflowToExecute
        )
            where TContext : IWorkflowContext<
                    IWorkflowInputContext,
                    IWorkflowOutputContext<TReturn>,
                    TReturn
                >
        {
            _logger.LogInformation("----------  Entering workflow execution: {WorkflowName} {WorkflowId}  ----------", workflowToExecute.Name, workflowToExecute.WorkflowRunId);
            try
            {
                await workflowToExecute.PreWorkflowRoutine();

                var allActivityBlocksToRun = workflowToExecute.ActivitiesToRun.FastArraySelect(x => (x.ExecutionType, x.ActivitesToRun.FastArraySelect(x => ToActualActivityToRun(x))));

                var completedActivitiesList = new List<CompletedActivityBlockToRun<object?, object?>>();

                foreach (var singleActivityBlock in allActivityBlocksToRun)
                {
                    var workflowActivityList = new List<CompletedWorkflowActivity<object?, object?>>();
                    var (exeType, funcsAndActivities) = singleActivityBlock;
                    if(exeType == ActivityBlockExecutionTypeEnum.Sync)
                    {
                        foreach(var funcAndActivity in funcsAndActivities)
                        {
                            var (singleFuncAndActualActivity, singleActivity) = funcAndActivity;
                            var (timeTakenForActivity, (activityResult, timesRetried)) = OperationTimerUtils.TimeWithResults(singleFuncAndActualActivity);
                            workflowActivityList.Add(new CompletedWorkflowActivity<object?, object?>(singleActivity, timesRetried, timeTakenForActivity, activityResult));
                        }
                    } else if (exeType == ActivityBlockExecutionTypeEnum.Async){
                        if(funcsAndActivities.Count() == 1)
                        {
                            var (singleFuncAndActualActivity, singleActivity) = funcsAndActivities.FirstOrDefault()!;
                            var (timeTakenForActivity, (activityResult, timesRetried)) = await OperationTimerUtils.TimeWithResultsAsync(singleFuncAndActualActivity);
                            workflowActivityList.Add(new CompletedWorkflowActivity<object?, object?>(singleActivity, timesRetried, timeTakenForActivity, activityResult));
                            continue;
                        }
                    }
                }
                
                
                
                _logger.LogInformation("----------  Exiting workflow execution: {WorkflowName} {WorkflowId}  -----------", workflowToExecute.Name, workflowToExecute.WorkflowRunId);
            }
            catch (WorkflowException e)
            {
                _logger.LogError(e, "Uncaught exception occoured during execution of workflow: {WorkflowName} {WorkflowId}", workflowToExecute.Name, workflowToExecute.WorkflowRunId);
                _logger.LogInformation("----------  Exiting workflow execution: {WorkflowName} {WorkflowId}  -----------", workflowToExecute.Name, workflowToExecute.WorkflowRunId);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Uncaught exception occoured during execution of workflow: {WorkflowName} {WorkflowId}", workflowToExecute.Name, workflowToExecute.WorkflowRunId);
                _logger.LogInformation("----------Exiting workflow execution: {WorkflowName} {WorkflowId}-----------", workflowToExecute.Name, workflowToExecute.WorkflowRunId);
                throw new WorkflowException($"Failed to execute {e.Message}", e);
            }
            //throw new NotImplementedException();
        }










        private 
             (Func<
            Task<(ActivityResultEnum ActivityResult, int TimesRetried)>
        >, IActivity<TActivityContextItem?, TActivityReturnItem?>)
         ToActualActivityToRun<TActivityContextItem, TActivityReturnItem>(
            ActivityToRun<TActivityContextItem, TActivityReturnItem> activity
        )
        {
            var resolvedActivity = (
                _serviceProvider.GetService(activity.ActivityType.ActualType)
                as IActivity<TActivityContextItem?, TActivityReturnItem?>
            ) ?? throw new WorkflowException(WorkflowConstants.CouldNotResolveActivity);


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
                        return (ActivityResult, retryCounter);
                    }
                    _logger.LogWarning("Activity {ActivityName}, {ActivityRunId} failed and took {TimeTaken}ms . On attempt: {AttemptNumber}", resolvedActivity.Name, resolvedActivity.ActivityRunId, timeTakenForAttempt.Milliseconds, retryCounter + 1);
                    if (retryCounter == timesToRetry - 1)
                    {
                        return (ActivityResult, retryCounter);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error with message {ExceptionMessage} in activity: {ActivityName}, {ActivityRunId}. On attempt: {AttemptNumber}", ex.Message, resolvedActivity.Name, resolvedActivity.ActivityRunId, retryCounter + 1);
                    if (retryOnException == false || retryCounter == timesToRetry - 1)
                    {
                        throw new WorkflowException(WorkflowConstants.CouldNotGetResultFromActivity, ex);
                    }
                }
            }
            throw new WorkflowException(WorkflowConstants.CouldNotGetResultFromActivity);
        }   
    }
}