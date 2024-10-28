using BT.Common.Helpers.TypeFor;
using BT.Common.WorkflowActivities.Abstract;
using BT.Common.WorkflowActivities.Completed;
using BT.Common.WorkflowActivities.Contexts;

namespace BT.Common.WorkflowActivities.Services.Abstract
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
