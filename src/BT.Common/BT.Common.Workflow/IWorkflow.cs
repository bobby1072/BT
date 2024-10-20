using BT.Common.Workflow.Contexts;
using BT.Common.Workflow.Models;

namespace BT.Common.Workflow
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
            IReadOnlyCollection<ActivityToRun<object>>
        > GetActivitiesToRun();

        ValueTask PreWorkflowRunProcess();
        ValueTask PostSuccessfulWorkflowRunProcess();
        ValueTask PostUnSuccessfulWorkflowRunProcess();
    }
}
