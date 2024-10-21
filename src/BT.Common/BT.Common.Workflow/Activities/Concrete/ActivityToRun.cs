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
        > ActivityType
        { get; init; }

        public DefaultActivityRetryAttribute? DefaultRetryAttribute =>
            ActivityType.ActualType.GetCustomAttribute<DefaultActivityRetryAttribute>();
        public TActivityContextItem? ContextItem { get; init; }
        public int? RetryCount { get; init; }
        public int? SecondsBetweenRetries { get; init; }
        public bool? RetryOnException { get; init; }
        public Func<TActivityContextItem, Func<TActivityContextItem?, Task<(ActivityResultEnum ActivityResult, TActivityReturnItem? ActualResult)>>, Task<(ActivityResultEnum ActivityResult, TActivityReturnItem? ActualResult)>>? ActivityWrapperFunc { get; init; }

        public ActivityToRun(
            TypeFor<IActivity<TActivityContextItem?, TActivityReturnItem?>> activityType,
            TActivityContextItem contextItem,
            int? retryCount = null,
            int? secondsBetweenRetries = null,
            bool? retryOnException = null,
            Func<TActivityContextItem, Func<TActivityContextItem?, Task<(ActivityResultEnum ActivityResult, TActivityReturnItem? ActualResult)>>, Task<(ActivityResultEnum ActivityResult, TActivityReturnItem? ActualResult)>>? activityWrapperFunc = null
        )
        {
            ActivityType = activityType;
            ContextItem = contextItem;
            RetryCount = retryCount;
            SecondsBetweenRetries = secondsBetweenRetries;
            RetryOnException = retryOnException;
            ActivityWrapperFunc = activityWrapperFunc;
        }

        public ActivityToRun(
            TypeFor<IActivity<TActivityContextItem?, TActivityReturnItem?>> activityType,
            TActivityContextItem contextItem,
            Func<TActivityContextItem, Func<TActivityContextItem?, Task<(ActivityResultEnum ActivityResult, TActivityReturnItem? ActualResult)>>, Task<(ActivityResultEnum ActivityResult, TActivityReturnItem? ActualResult)>>? activityWrapperFunc = null,
            int? retryCount = null,
            int? secondsBetweenRetries = null,
            bool? retryOnException = null
        )
        {
            ActivityType = activityType;
            ContextItem = contextItem;
            RetryCount = retryCount;
            SecondsBetweenRetries = secondsBetweenRetries;
            RetryOnException = retryOnException;
            ActivityWrapperFunc = activityWrapperFunc;
        }
    }
}
