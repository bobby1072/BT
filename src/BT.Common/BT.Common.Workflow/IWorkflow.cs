using AiTrainer.Web.Workflow.Contexts;
using AiTrainer.Web.Workflow.Models;

namespace AiTrainer.Web.Workflow
{
    public interface IWorkflow<TContext, TReturn, TActivityContextItem>
        where TContext : IWorkflowContext<
                IWorkflowInputContext,
                IWorkflowOutputContext<TReturn>,
                TReturn
            >
    {
        TContext Context { get; init; }

        IReadOnlyCollection<
            IReadOnlyCollection<ActivityToRun<TActivityContextItem>>
        > ActivitiesToRun { get; }

        ValueTask PostSuccessfulWorkflowRunProcess();
        ValueTask PostUnSuccessfulWorkflowRunProcess();
    }
}
