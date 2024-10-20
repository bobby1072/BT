using BT.Common.Workflow.Contexts;

namespace BT.Common.Workflow.Services.Abstract
{
    public interface IWorkflowExecuterService
    {
        //Task<TReturn> ExcecuteAsync<TInputContext, TOutputContext,TContext, TReturn>(IWorkflow<TContext, TReturn> workflowToExecute) where TContext: IWorkflowContext<TInputContext, TOutputContext, TReturn>;
    }
}
