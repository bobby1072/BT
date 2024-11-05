using BT.Common.WorkflowActivities.Activities.Concrete;
using BT.Common.WorkflowActivities.Contexts;

namespace BT.Common.WorkflowActivities.Abstract
{
    public interface IWorkflow<TContext, TInputContext, TOutputContext, TReturn>
        where TContext : IWorkflowContext<
                TInputContext,
                TOutputContext,
                TReturn
            >
        where TInputContext : IWorkflowInputContext
        where TOutputContext : IWorkflowOutputContext<TReturn>
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
