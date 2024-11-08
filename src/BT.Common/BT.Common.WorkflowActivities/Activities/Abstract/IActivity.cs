using BT.Common.WorkflowActivities.Activities.Concrete;
using BT.Common.WorkflowActivities.Contexts;

namespace BT.Common.WorkflowActivities.Activities.Abstract
{
    public interface IActivity<TParam, TReturn> where TParam : ActivityContextItem where TReturn : ActivityReturnItem
    {
        Guid ActivityRunId { get; }
        string Name { get; }
        string Description { get; }

        Task<(ActivityResultEnum ActivityResult, TReturn ActualResult)> ExecuteAsync(
            TParam workflowContextItem
        );
    }
}
