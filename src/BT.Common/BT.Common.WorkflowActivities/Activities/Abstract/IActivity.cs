using BT.Common.WorkflowActivities.Activities.Concrete;

namespace BT.Common.WorkflowActivities.Activities.Abstract
{
    public interface IActivity<TParam, TReturn>
    {
        Guid ActivityRunId { get; }
        string Name { get; }
        string Description { get; }

        Task<(ActivityResultEnum ActivityResult, TReturn? ActualResult)> ExecuteAsync(
            TParam? workflowContextItem
        );
    }
}
