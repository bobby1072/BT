using BT.Common.WorkflowActivities.Activities.Concrete;
using BT.Common.WorkflowActivities.Contexts;

namespace BT.Common.WorkflowActivities.Abstract
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
