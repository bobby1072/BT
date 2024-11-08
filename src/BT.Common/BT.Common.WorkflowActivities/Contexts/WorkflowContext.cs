namespace BT.Common.WorkflowActivities.Contexts
{
    public abstract record WorkflowContext<TInputContext, TOutputContext, TReturn>
        where TInputContext : WorkflowInputContext
        where TOutputContext : WorkflowOutputContext<TReturn>
    {
        public abstract TInputContext Input { get; init; }
        public abstract TOutputContext Output { get; init; }
    }
}
