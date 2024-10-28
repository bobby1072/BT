using BT.Common.WorkflowActivities.Activities.Concrete;

namespace BT.Common.WorkflowActivities.Activities.Abstract
{
    public abstract class BaseActivity<TParam, TReturn> : IActivity<TParam, TReturn>
    {
        public Guid ActivityRunId { get; } = Guid.NewGuid();
        public string Name =>
            GetType().Name.Replace("Activity", "", StringComparison.CurrentCultureIgnoreCase);
        public abstract string Description { get; }
        public abstract Task<(
            ActivityResultEnum ActivityResult,
            TReturn? ActualResult
        )> ExecuteAsync(TParam? workflowContextItem);
    }
}
