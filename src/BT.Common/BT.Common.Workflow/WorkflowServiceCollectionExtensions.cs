using BT.Common.Workflow.Abstract;
using BT.Common.Workflow.Activities.Abstract;
using BT.Common.Workflow.Contexts;
using BT.Common.Workflow.Services.Abstract;
using BT.Common.Workflow.Services.Concrete;
using Microsoft.Extensions.DependencyInjection;

namespace BT.Common.Workflow
{
    public static class WorkflowServiceCollectionExtensions
    {
        public static IServiceCollection AddActivity<TActivityInterface, TActivityActual>(this IServiceCollection serviceCollection)
             where TActivityInterface : class, IActivity<object?, object?>
             where TActivityActual : class, TActivityInterface
        {

            serviceCollection
                .AddTransient<TActivityInterface, TActivityActual>();

            return serviceCollection;
        }

        public static IServiceCollection AddWorkflow<TWorkflowInterface, TWorkflowActual>(this IServiceCollection serviceCollection)
            where TWorkflowInterface : class, IWorkflow<IWorkflowContext<IWorkflowInputContext, IWorkflowOutputContext<object?>, object?>, object?>
            where TWorkflowActual : class, TWorkflowInterface
        {
            serviceCollection
                .AddTransient<TWorkflowInterface, TWorkflowActual>();

            return serviceCollection;
        }

        public static IServiceCollection AddWorkflowServices(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddTransient<IWorkflowExecuterService, WorkflowExecuterService>();

            return serviceCollection;
        }


    }
}
