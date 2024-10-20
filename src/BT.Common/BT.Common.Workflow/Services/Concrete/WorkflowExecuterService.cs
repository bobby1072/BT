using BT.Common.Workflow.Abstract;
using BT.Common.Workflow.Activities.Abstract;
using BT.Common.Workflow.Activities.Concrete;
using BT.Common.Workflow.Common;
using BT.Common.Workflow.Concrete;
using BT.Common.Workflow.Contexts;
using BT.Common.Workflow.Exceptions;
using BT.Common.Workflow.Services.Abstract;
using Microsoft.Extensions.Logging;

namespace BT.Common.Workflow.Services.Concrete
{
    public class WorkflowExecuterService
    // : IWorkflowExecuterService
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
        // public async Task<(CompletedWorkflowActualActivityResult<TContext, TReturn>, TReturn?)> ExecuteAsync<TContext, TReturn>(
        //     IWorkflow<TContext, TReturn> workflowToExecute
        // )
        //     where TContext : IWorkflowContext<
        //             IWorkflowInputContext,
        //             IWorkflowOutputContext<TReturn>,
        //             TReturn
        //         >
        // {



        // }










        private IEnumerable<
            ActualActivityToRun<TActivityContextItem, TActivityReturnItem>
        > ToActualActivityToRun<TActivityContextItem, TActivityReturnItem>(
            IEnumerable<ActivityToRun<TActivityContextItem, TActivityReturnItem>> activity
        )
        {
            foreach (var item in activity)
            {
                yield return ToActualActivityToRun(item);
            }
        }

        private ActualActivityToRun<
            TActivityContextItem,
            TActivityReturnItem
        > ToActualActivityToRun<TActivityContextItem, TActivityReturnItem>(
            ActivityToRun<TActivityContextItem, TActivityReturnItem> activity
        )
        {
            var resolvedActivity = ResolveActivity(activity);

            return new ActualActivityToRun<TActivityContextItem, TActivityReturnItem>(
                resolvedActivity,
                () => ActualFunc(activity, resolvedActivity)
            );
        }

        private async Task<(
            ActivityResultEnum ActivityResult,
            TActivityReturnItem? ActualResult,
            int TimesRetried
        )> ActualFunc<TActivityContextItem, TActivityReturnItem>(
            ActivityToRun<TActivityContextItem, TActivityReturnItem> activity,
            IActivity<TActivityContextItem?, TActivityReturnItem?> resolvedActivity
        )
        {
            var timesToRetry =
                activity.RetryCount ?? activity.DefaultRetryAttribute?.RetryCount ?? 1;

            var secondsBetweenRetries =
                activity.SecondsBetweenRetries
                ?? activity.DefaultRetryAttribute?.SecondsBetweenRetries
                ?? 0;

            var retryOnException =
                activity.RetryOnException
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
                        mainResultFunc = (x) => activity.ActivityWrapperFunc.Invoke(x!, resolvedActivity.ExecuteAsync);
                    }

                    var (ActivityResult, ActualResult) = await mainResultFunc.Invoke(activity.ContextItem);


                    if (ActivityResult == ActivityResultEnum.Success)
                    {
                        _logger.LogInformation("Activity {ActivityName}, {ActivityRunId} executed successfully. On attempt: {AttemptNumber}", resolvedActivity.Name, resolvedActivity.ActivityRunId, retryCounter + 1);
                        return (ActivityResult, ActualResult, retryCounter);
                    }
                    _logger.LogWarning("Activity {ActivityName}, {ActivityRunId} failed. On attempt: {AttemptNumber}", resolvedActivity.Name, resolvedActivity.ActivityRunId, retryCounter + 1);
                    if (retryCounter == timesToRetry - 1)
                    {
                        return (ActivityResult, ActualResult, retryCounter);
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

        private IActivity<TActivityContextItem?, TActivityReturnItem?> ResolveActivity<
            TActivityContextItem,
            TActivityReturnItem
        >(
            ActivityToRun<TActivityContextItem, TActivityReturnItem> activity
        ) =>
            (
                _serviceProvider.GetService(activity.ActivityType.ActualType)
                as IActivity<TActivityContextItem?, TActivityReturnItem?>
            ) ?? throw new WorkflowException(WorkflowConstants.CouldNotResolveActivity);
    }
}