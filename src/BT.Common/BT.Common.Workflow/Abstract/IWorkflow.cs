using BT.Common.Workflow.Contexts;
using BT.Common.Workflow.Models;

namespace BT.Common.Workflow.Abstract
{
    public interface IWorkflow<TContext, TReturn>
        where TContext : IWorkflowContext<
                IWorkflowInputContext,
                IWorkflowOutputContext<TReturn>,
                TReturn
            >
    {
        Guid WorkflowRunId { get; }
        string Name { get; }
        TContext Context { get; init; }

        IReadOnlyCollection<
            IReadOnlyCollection<ActivityToRun<object?, object?>>
        > ActivitiesToRun { get; }

        Task PreWorkflowRunProcess();
        Task PostSuccessfulWorkflowRunProcess();
        Task PostUnSuccessfulWorkflowRunProcess();
    }
}
