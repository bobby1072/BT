using BT.Common.WorkflowActivities.Contexts;

namespace BT.Common.WorkflowActivities.Activities.Concrete
{
    public class ActivityBlockToRun
    {
        public required IReadOnlyCollection<ActivityToRun<ActivityContextItem, ActivityReturnItem>> ActivitesToRun { get; init; }
        public required ActivityBlockExecutionTypeEnum ExecutionType { get; init; }
    }
}
