using BT.Common.Workflow.Activities.Concrete;

namespace BT.Common.Workflow.Activities.Abstract
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
