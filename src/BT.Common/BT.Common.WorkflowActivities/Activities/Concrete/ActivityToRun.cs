using System.Reflection;
using BT.Common.Helpers.TypeFor;
using BT.Common.WorkflowActivities.Activities.Abstract;
using BT.Common.WorkflowActivities.Activities.Attributes;

namespace BT.Common.WorkflowActivities.Activities.Concrete
{
    public class ActivityToRun<TActivityContextItem, TActivityReturnItem>
    {
        public TypeFor<
            IActivity<TActivityContextItem?, TActivityReturnItem?>
        > ActivityType
        { get; init; }

        public TActivityContextItem? ContextItem { get; init; }
        public Func<TActivityContextItem?, Func<TActivityContextItem?, Task<(ActivityResultEnum ActivityResult, TActivityReturnItem? ActualResult)>>, Task<(ActivityResultEnum ActivityResult, TActivityReturnItem? ActualResult)>>? ActivityWrapperFunc { get; init; }




        public DefaultActivityRetryAttribute? DefaultRetryAttribute =>
            ActivityType.ActualType.GetCustomAttribute<DefaultActivityRetryAttribute>();
        public int? OverrideRetryCount { get; init; }
        public int? OverrideSecondsBetweenRetries { get; init; }
        public bool? OverrideRetryOnException { get; init; }




        public ActivityToRun(
            TypeFor<IActivity<TActivityContextItem?, TActivityReturnItem?>> activityType,
            TActivityContextItem? contextItem,
            int? retryCount = null,
            int? secondsBetweenRetries = null,
            bool? retryOnException = null,
            Func<TActivityContextItem?, Func<TActivityContextItem?, Task<(ActivityResultEnum ActivityResult, TActivityReturnItem? ActualResult)>>, Task<(ActivityResultEnum ActivityResult, TActivityReturnItem? ActualResult)>>? activityWrapperFunc = null
        )
        {
            ActivityType = activityType;
            ContextItem = contextItem;
            OverrideRetryCount = retryCount;
            OverrideSecondsBetweenRetries = secondsBetweenRetries;
            OverrideRetryOnException = retryOnException;
            ActivityWrapperFunc = activityWrapperFunc;
        }

        public ActivityToRun(
            TypeFor<IActivity<TActivityContextItem?, TActivityReturnItem?>> activityType,
            TActivityContextItem? contextItem,
            Func<TActivityContextItem?, Func<TActivityContextItem?, Task<(ActivityResultEnum ActivityResult, TActivityReturnItem? ActualResult)>>, Task<(ActivityResultEnum ActivityResult, TActivityReturnItem? ActualResult)>>? activityWrapperFunc = null,
            int? retryCount = null,
            int? secondsBetweenRetries = null,
            bool? retryOnException = null
        )
        {
            ActivityType = activityType;
            ContextItem = contextItem;
            OverrideRetryCount = retryCount;
            OverrideSecondsBetweenRetries = secondsBetweenRetries;
            OverrideRetryOnException = retryOnException;
            ActivityWrapperFunc = activityWrapperFunc;
        }
    }
}
