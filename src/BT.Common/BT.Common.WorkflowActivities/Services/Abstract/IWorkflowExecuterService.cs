using BT.Common.Helpers.TypeFor;
using BT.Common.WorkflowActivities.Abstract;
using BT.Common.WorkflowActivities.Completed;
using BT.Common.WorkflowActivities.Contexts;

namespace BT.Common.WorkflowActivities.Services.Abstract
{
    public interface IWorkflowExecuterService
    {
        Task<CompletedWorkflow<TContext, TInputContext, TOutputContext, TReturn>> ExecuteAsync<TContext, TInputContext, TOutputContext, TReturn>(
            TypeFor<IWorkflow<TContext, TInputContext, TOutputContext, TReturn>> workflowToExecute
        )
        where TContext : IWorkflowContext<
                TInputContext,
                TOutputContext,
                TReturn
            >
        where TInputContext : IWorkflowInputContext
        where TOutputContext : IWorkflowOutputContext<TReturn>;
    }
}
