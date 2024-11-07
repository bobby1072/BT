using BT.Common.WorkflowActivities.Activities.Concrete;
using BT.Common.WorkflowActivities.Contexts;

namespace BT.Common.WorkflowActivities.Abstract
{
    public abstract class BaseWorkflow<TContext, TInputContext, TOutputContext, TReturn> : IWorkflow<TContext, TInputContext, TOutputContext, TReturn>
        where TContext : WorkflowContext<
                TInputContext,
                TOutputContext,
                TReturn
            >
        where TInputContext : WorkflowInputContext
        where TOutputContext : WorkflowOutputContext<TReturn>
    {
        public Guid WorkflowRunId { get; } = Guid.NewGuid();
        public string Name =>
            GetType().Name.Replace("Workflow", "", StringComparison.CurrentCultureIgnoreCase);
        public abstract string Description { get; }
        public required TContext Context { get; init; }
        public abstract IReadOnlyCollection<ActivityBlockToRun> ActivitiesToRun { get; }
        public virtual Task PreWorkflowRoutine() => Task.CompletedTask;
        public virtual Task PostSuccessfulWorkflowRoutine() => Task.CompletedTask;
        public virtual Task PostUnsuccessfulWorkflowRoutine() => Task.CompletedTask;
    }
}
