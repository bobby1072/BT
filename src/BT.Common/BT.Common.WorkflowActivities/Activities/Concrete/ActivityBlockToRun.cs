namespace BT.Common.WorkflowActivities.Activities.Concrete
{
    public class ActivityBlockToRun
    {
        public required IReadOnlyCollection<ActivityToRun<object?, object?>> ActivitesToRun { get; init; }
        public required ActivityBlockExecutionTypeEnum ExecutionType { get; init; }
    }
}
