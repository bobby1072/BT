namespace BT.Common.Workflow.Activities.Concrete
{
    public sealed class ActivityBlockToRun
    {
        public IReadOnlyCollection<ActivityToRun<object?, object?>> ActivitesToRun { get; init; }
        public ActivityBlockExecutionTypeEnum ExecutionType { get; init; }
        public ActivityBlockToRun(
            IReadOnlyCollection<ActivityToRun<object?, object?>> activitesToRun, ActivityBlockExecutionTypeEnum executionType)
        {
            ActivitesToRun = activitesToRun;
            ExecutionType = executionType;
        }
    }
}
