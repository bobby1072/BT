using BT.Common.Workflow.Models;

namespace BT.Common.Workflow.Activities
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
