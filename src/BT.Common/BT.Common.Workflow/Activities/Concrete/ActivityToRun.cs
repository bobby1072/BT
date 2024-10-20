using System.Reflection;
using BT.Common.Helpers.TypeFor;
using BT.Common.Workflow.Activities.Abstract;
using BT.Common.Workflow.Activities.Attributes;

namespace BT.Common.Workflow.Activities.Concrete
{
    public sealed record ActivityToRun<TActivityContextItem, TActivityReturnItem>
    {
        public TypeFor<
            IActivity<TActivityContextItem?, TActivityReturnItem?>
        > ActivityType { get; init; }

        public DefaultActivityRetryAttribute? DefaultRetryAttribute =>
            ActivityType.ActualType.GetCustomAttribute<DefaultActivityRetryAttribute>();
        public TActivityContextItem? ContextItem { get; init; }
        public int? RetryCount { get; init; }
        public int? SecondsBetweenRetries { get; init; }
        public Func<
            TActivityContextItem?,
            Task<TActivityContextItem?>
        >? PreActivityAction { get; init; }
        public Func<
            ActivityResultEnum,
            TActivityReturnItem?,
            Task<(ActivityResultEnum ActivityResult, TActivityReturnItem? ActualResult)>
        >? PostActivityAction { get; init; }

        public ActivityToRun(
            TypeFor<IActivity<TActivityContextItem?, TActivityReturnItem?>> activityType,
            TActivityContextItem contextItem,
            int? retryCount = null,
            int? secondsBetweenRetries = null,
            Func<TActivityContextItem?, Task<TActivityContextItem?>>? preActivityAction = null,
            Func<
                ActivityResultEnum,
                TActivityReturnItem?,
                Task<(ActivityResultEnum ActivityResult, TActivityReturnItem? ActualResult)>
            >? postActivityAction = null
        )
        {
            ActivityType = activityType;
            RetryCount = retryCount;
            ContextItem = contextItem;
            SecondsBetweenRetries = secondsBetweenRetries;
            PreActivityAction = preActivityAction;
            PostActivityAction = postActivityAction;
        }

        public ActivityToRun(
            TypeFor<IActivity<TActivityContextItem?, TActivityReturnItem?>> activityType,
            TActivityContextItem contextItem,
            Func<TActivityContextItem?, Task<TActivityContextItem?>>? preActivityAction = null,
            Func<
                ActivityResultEnum,
                TActivityReturnItem?,
                Task<(ActivityResultEnum ActivityResult, TActivityReturnItem? ActualResult)>
            >? postActivityAction = null,
            int? retryCount = null,
            int? secondsBetweenRetries = null
        )
        {
            ActivityType = activityType;
            RetryCount = retryCount;
            ContextItem = contextItem;
            SecondsBetweenRetries = secondsBetweenRetries;
            PreActivityAction = preActivityAction;
            PostActivityAction = postActivityAction;
        }

        public ActivityToRun(
            TActivityContextItem contextItem,
            Func<TActivityContextItem?, Task<TActivityContextItem?>> preActivityAction,
            TypeFor<IActivity<TActivityContextItem?, TActivityReturnItem?>> activityType,
            Func<
                ActivityResultEnum,
                TActivityReturnItem?,
                Task<(ActivityResultEnum ActivityResult, TActivityReturnItem? ActualResult)>
            >? postActivityAction = null,
            int? retryCount = null,
            int? secondsBetweenRetries = null
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
