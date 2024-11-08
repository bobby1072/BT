using BT.Common.Helpers.TypeFor;
using BT.Common.WorkflowActivities.Activities.Abstract;
using BT.Common.WorkflowActivities.Activities.Attributes;
using BT.Common.WorkflowActivities.Contexts;
using System.Reflection;

namespace BT.Common.WorkflowActivities.Activities.Concrete
{
    public class ActivityToRun<TActivityContextItem, TActivityReturnItem> where TActivityContextItem: ActivityContextItem where TActivityReturnItem: ActivityReturnItem
    {
        public required TypeFor<
            IActivity<TActivityContextItem, TActivityReturnItem>
        > ActivityType
        { get; init; }

        public required TActivityContextItem ContextItem { get; init; }
        public Func<TActivityContextItem, Func<TActivityContextItem, Task<(ActivityResultEnum ActivityResult, TActivityReturnItem ActualResult)>>, Task<(ActivityResultEnum ActivityResult, TActivityReturnItem ActualResult)>>? ActivityWrapperFunc { get; init; }




        public DefaultActivityRetryAttribute? DefaultRetryAttribute =>
            ActivityType.ActualType.GetCustomAttribute<DefaultActivityRetryAttribute>();
        public int? OverrideRetryCount { get; init; }
        public int? OverrideSecondsBetweenRetries { get; init; }
        public bool? OverrideRetryOnException { get; init; }
    }
}
