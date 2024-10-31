﻿using BT.Common.WorkflowActivities.Abstract;
using BT.Common.WorkflowActivities.Activities.Abstract;
using BT.Common.WorkflowActivities.Contexts;
using BT.Common.WorkflowActivities.Services.Abstract;
using BT.Common.WorkflowActivities.Services.Concrete;
using Microsoft.Extensions.DependencyInjection;

namespace BT.Common.WorkflowActivities
{
    public static class WorkflowServiceCollectionExtensions
    {
        public static IServiceCollection AddActivity<TActivityActual, TParam, TReturn>(this IServiceCollection serviceCollection)
            where TActivityActual:class,  IActivity<TParam?, TReturn?>
        {
            serviceCollection
                .AddTransient<IActivity<TParam?, TReturn?>,TActivityActual>();

            return serviceCollection;
        }

        public static IServiceCollection AddWorkflow<TWorkflowActual, TReturn>(this IServiceCollection serviceCollection)
            where TWorkflowActual : class, IWorkflow<IWorkflowContext<IWorkflowInputContext, IWorkflowOutputContext<TReturn?>, TReturn?>, TReturn?>
        {
            serviceCollection
                .AddTransient<IWorkflow<IWorkflowContext<IWorkflowInputContext, IWorkflowOutputContext<TReturn?>, TReturn?>, TReturn?>, TWorkflowActual>();

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
