namespace BT.Common.WorkflowActivities.Contexts
{
    public abstract class WorkflowOutputContext<TReturn>
    {
        public abstract TReturn? ReturnObject { get; set; }
        public WorkflowResultEnum WorkflowResultEnum { get; set; } = WorkflowResultEnum.Satrted;
    }
}
