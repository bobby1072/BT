using BT.Common.Workflow.Activities.Concrete;
using BT.Common.Workflow.Contexts;

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

        Task PreWorkflowRoutine();
        Task PostSuccessfulWorkflowRoutine();
        Task PostUnsuccessfulWorkflowRoutine();
    }
}
