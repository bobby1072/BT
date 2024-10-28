﻿namespace BT.Common.WorkflowActivities.Contexts
{
    public interface IWorkflowContext<TInputContext, TOutputContext, TReturn>
        where TInputContext : IWorkflowInputContext
        where TOutputContext : IWorkflowOutputContext<TReturn>
    {
        TInputContext Input { get; init; }
        TOutputContext Output { get; init; }
    }
}
