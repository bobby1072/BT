using BT.Common.Workflow.Models;

namespace BT.Common.Workflow.Activities
{
    public interface IActivity<T>
    {
        public string Name { get; }
        public string Description { get; }
        Task<ActivityResultEnum> ExecuteAsync(T workflowContexItem);
    }
}
