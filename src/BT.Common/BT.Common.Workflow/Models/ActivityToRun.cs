using BT.Common.Helpers.TypeFor;
using BT.Common.Workflow.Activities;

namespace BT.Common.Workflow.Models
{
    public sealed record ActivityToRun<TActivityContextItem, TActivityReturnItem>
    {
        public TypeFor<
            IActivity<TActivityContextItem?, TActivityReturnItem?>
        > ActivityType { get; init; }
        public TActivityContextItem? ContextItem { get; init; }
        public int RetryCount { get; init; }
        public int SecondsBetweenRetries { get; init; }
        public Func<TActivityContextItem?, Task>? PreActivityAction { get; init; }
        public Func<TActivityReturnItem?, Task>? PostActivityAction { get; init; }

        public ActivityToRun(
            TypeFor<IActivity<TActivityContextItem?, TActivityReturnItem?>> activityType,
            TActivityContextItem contextItem,
            int retryCount = 0,
            int secondsBetweenRetries = 0,
            Func<TActivityContextItem?, Task>? preActivityAction = null,
            Func<TActivityReturnItem?, Task>? postActivityAction = null
        )
        {
            ActivityType = activityType;
            RetryCount = retryCount;
            ContextItem = contextItem;
            SecondsBetweenRetries = secondsBetweenRetries;
            PreActivityAction = preActivityAction;
            PostActivityAction = postActivityAction;
        }
    }
}
