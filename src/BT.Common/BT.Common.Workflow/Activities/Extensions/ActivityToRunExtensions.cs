using BT.Common.Workflow.Activities.Abstract;
using BT.Common.Workflow.Activities.Concrete;
using BT.Common.Workflow.Common;
using BT.Common.Workflow.Exceptions;

namespace BT.Common.Workflow.Activities.Extensions
{
    internal static class ActivityToRunExtensions
    {
        public static IEnumerable<
            ActualActivityToRun<TActivityContextItem, TActivityReturnItem>
        > ToActualActivityToRun<TActivityContextItem, TActivityReturnItem>(
            this IEnumerable<ActivityToRun<TActivityContextItem, TActivityReturnItem>> activity,
            IServiceProvider serviceProvider
        )
        {
            foreach (var item in activity)
            {
                yield return item.ToActualActivityToRun(serviceProvider);
            }
        }

        public static ActualActivityToRun<
            TActivityContextItem,
            TActivityReturnItem
        > ToActualActivityToRun<TActivityContextItem, TActivityReturnItem>(
            this ActivityToRun<TActivityContextItem, TActivityReturnItem> activity,
            IServiceProvider serviceProvider
        )
        {
            var resolvedActivity = activity.ResolveActivity(serviceProvider);

            return new ActualActivityToRun<TActivityContextItem, TActivityReturnItem>(
                resolvedActivity,
                () => activity.ActualFunc(resolvedActivity)
            );
        }

        private static async Task<(
            ActivityResultEnum ActivityResult,
            TActivityReturnItem? ActualResult
        )> ActualFunc<TActivityContextItem, TActivityReturnItem>(
            this ActivityToRun<TActivityContextItem, TActivityReturnItem> activity,
            IActivity<TActivityContextItem?, TActivityReturnItem?> resolvedActivity
        )
        {
            var timesToRetry =
                activity.RetryCount ?? activity.DefaultRetryAttribute?.RetryCount ?? 1;

            var secondsBetweenRetries =
                activity.SecondsBetweenRetries
                ?? activity.DefaultRetryAttribute?.SecondsBetweenRetries
                ?? 0;

            var contextItem = activity.ContextItem;

            for (int retryCounter = 0; retryCounter < timesToRetry; retryCounter++)
            {
                if (retryCounter > 0 && secondsBetweenRetries > 0)
                {
                    await Task.Delay(secondsBetweenRetries * 1000);
                }
                try
                {
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
                    return mainResult;
                }
                catch { }
            }
            throw new WorkflowException(WorkflowConstants.CouldNotGetResultFromActivity);
        }

        private static IActivity<TActivityContextItem?, TActivityReturnItem?> ResolveActivity<
            TActivityContextItem,
            TActivityReturnItem
        >(
            this ActivityToRun<TActivityContextItem, TActivityReturnItem> activity,
            IServiceProvider serviceProvider
        ) =>
            (
                serviceProvider.GetService(activity.ActivityType.ActualType)
                as IActivity<TActivityContextItem?, TActivityReturnItem?>
            ) ?? throw new WorkflowException(WorkflowConstants.CouldNotResolveActivity);
    }
}
