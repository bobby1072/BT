using BT.Common.WorkflowActivities.Activities.Concrete;
using BT.Common.WorkflowActivities.Contexts;

namespace BT.Common.WorkflowActivities.Abstract
{
    public abstract class BaseWorkflow<TContext, TReturn> : IWorkflow<TContext, TReturn>
        where TContext : IWorkflowContext<
                IWorkflowInputContext,
                IWorkflowOutputContext<TReturn>,
                TReturn
            >
    {
        public Guid WorkflowRunId { get; } = Guid.NewGuid();
        public string Name =>
            GetType().Name.Replace("Workflow", "", StringComparison.CurrentCultureIgnoreCase);
        public abstract string Description { get; }
        public TContext Context { get; init; }
        public abstract IReadOnlyCollection<ActivityBlockToRun> ActivitiesToRun { get; }
        public BaseWorkflow(TContext context)
        {
            Context = context;
        }
        public virtual Task PreWorkflowRoutine() => Task.CompletedTask;
        public virtual Task PostSuccessfulWorkflowRoutine() => Task.CompletedTask;
        public virtual Task PostUnsuccessfulWorkflowRoutine() => Task.CompletedTask;
    }
}
