namespace BT.Common.WorkflowActivities.Activities.Concrete
{
    public class ActivityBlockToRun
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
