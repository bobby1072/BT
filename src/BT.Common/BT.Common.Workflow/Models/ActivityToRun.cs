using AiTrainer.Web.Workflow.Activities;
using BT.Common.Helpers.TypeFor;

namespace AiTrainer.Web.Workflow.Models
{
    public sealed record ActivityToRun<TActivityContextItem>
    {
        public TypeFor<IActivity<TActivityContextItem>> ActivityType { get; init; }
        public int RetryCount { get; init; }
        public int SecondsBetweenRetries { get; init; }
        public Action<TActivityContextItem>? PreActivityAction { get; init; }
        public Action<TActivityContextItem>? PostActivityAction { get; init; }

        public ActivityToRun(
            TypeFor<IActivity<TActivityContextItem>> activityType,
            int retryCount = 0,
            int secondsBetweenRetries = 0,
            Action<TActivityContextItem>? preActivityAction = null,
            Action<TActivityContextItem>? postActivityAction = null
        )
        {
            ActivityType = activityType;
            RetryCount = retryCount;
            SecondsBetweenRetries = secondsBetweenRetries;
            PreActivityAction = preActivityAction;
            PostActivityAction = postActivityAction;
        }
    }
}
