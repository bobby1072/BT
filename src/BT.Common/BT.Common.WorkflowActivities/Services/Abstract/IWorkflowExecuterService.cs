using BT.Common.Helpers.TypeFor;
using BT.Common.Workflow.Abstract;
using BT.Common.Workflow.Completed;
using BT.Common.Workflow.Contexts;

namespace BT.Common.Workflow.Services.Abstract
{
    public interface IWorkflowExecuterService
    {
        Task<CompletedWorkflow<TContext, TReturn>> ExecuteAsync<TContext, TReturn>(
            TypeFor<IWorkflow<TContext, TReturn>> workflowToExecute
        )
            where TContext : IWorkflowContext<
                    IWorkflowInputContext,
                    IWorkflowOutputContext<TReturn>,
                    TReturn
                >;
    }
}
