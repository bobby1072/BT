namespace BT.Common.WorkflowActivities.Contexts
{
    public abstract class WorkflowContext<TInputContext, TOutputContext, TReturn>
        where TInputContext : WorkflowInputContext
        where TOutputContext : WorkflowOutputContext<TReturn>
    {
        public abstract TInputContext Input { get; init; }
        public abstract TOutputContext Output { get; init; }
    }
}
