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
        string Description { get; }
        TContext Context { get; init; }
        IReadOnlyCollection<ActivityBlockToRun> ActivitiesToRun { get; }


        Task PreWorkflowRoutine();
        Task PostSuccessfulWorkflowRoutine();
        Task PostUnsuccessfulWorkflowRoutine();
    }
}
