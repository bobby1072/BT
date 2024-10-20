using BT.Common.Workflow.Abstract;
using BT.Common.Workflow.Contexts;

namespace BT.Common.Workflow.Services.Abstract
{
    public interface IWorkflowExecuterService
    {
        Task<TReturn?> ExecuteAsync<TContext, TReturn>(
            IWorkflow<TContext, TReturn> workflowToExecute
        )
            where TContext : IWorkflowContext<
                    IWorkflowInputContext,
                    IWorkflowOutputContext<TReturn>,
                    TReturn
                >;
    }
}
