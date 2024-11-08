namespace BT.Common.WorkflowActivities.Contexts
{
    public abstract record WorkflowOutputContext<TReturn>
    {
        public abstract TReturn? ReturnObject { get; set; }
    }
}
