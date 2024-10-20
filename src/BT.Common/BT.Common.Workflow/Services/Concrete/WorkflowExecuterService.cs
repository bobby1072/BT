using System.Security.AccessControl;
using BT.Common.Workflow.Abstract;
using BT.Common.Workflow.Activities.Abstract;
using BT.Common.Workflow.Activities.Concrete;
using BT.Common.Workflow.Common;
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
        // public async Task<TReturn?> ExecuteAsync<TContext, TReturn>(
        //     IWorkflow<TContext, TReturn> workflowToExecute
        // )
        //     where TContext : IWorkflowContext<
        //             IWorkflowInputContext,
        //             IWorkflowOutputContext<TReturn>,
        //             TReturn
        //         >{
        //         }


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

            var retryOnFailedActivityResult =
                activity.RetryOnFailedActivityResult
                ?? activity.DefaultRetryAttribute?.RetryOnFailedActivityResult
                ?? true;

            var contextItem = activity.ContextItem;
            (ActivityResultEnum ActivityResult, TActivityReturnItem? ActualResult) finalActivityResult;
            for (int retryCounter = 0; retryCounter < timesToRetry; retryCounter++)
            {
                try
                {
                    if (retryCounter > 0 && secondsBetweenRetries > 0)
                    {
                        await Task.Delay(secondsBetweenRetries * 1000);
                    }

                    if (activity.PreActivityAction is not null)
                    {
                        contextItem = await activity.PreActivityAction.Invoke(activity.ContextItem);
                    }

                    var mainResult = await resolvedActivity.ExecuteAsync(contextItem);

                    if (activity.PostActivityAction is not null)
                    {
                        mainResult = await activity.PostActivityAction.Invoke(
                            mainResult.ActivityResult,
                            mainResult.ActualResult
                        );
                    }

                    finalActivityResult = mainResult;

                    if (finalActivityResult.ActivityResult == ActivityResultEnum.Success ||
                        retryCounter == timesToRetry - 1)
                    {
                        return (finalActivityResult.ActivityResult, finalActivityResult.ActualResult, retryCounter);
                    }
                    else if (retryOnFailedActivityResult == false)
                    {
                        return (ActivityResultEnum.Fail, default, retryCounter);
                    }
                }
                catch
                {
                    if (retryOnException == false)
                    {
                        break;
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