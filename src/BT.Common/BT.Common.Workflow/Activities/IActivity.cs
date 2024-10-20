using BT.Common.Workflow.Models;

namespace BT.Common.Workflow.Activities
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
