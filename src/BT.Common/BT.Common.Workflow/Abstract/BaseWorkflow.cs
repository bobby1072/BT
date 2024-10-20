using BT.Common.Workflow.Contexts;
using BT.Common.Workflow.Models;

namespace BT.Common.Workflow.Abstract
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
        public TContext Context { get; init; }

        public abstract IReadOnlyCollection<
            IReadOnlyCollection<ActivityToRun<object?, object?>>
        > ActivitiesToRun { get; }

        public BaseWorkflow(TContext context)
        {
            Context = context;
        }

        public virtual Task PreWorkflowRunProcess() => Task.CompletedTask;

        public virtual Task PostSuccessfulWorkflowRunProcess() => Task.CompletedTask;

        public virtual Task PostUnSuccessfulWorkflowRunProcess() => Task.CompletedTask;
    }
}
