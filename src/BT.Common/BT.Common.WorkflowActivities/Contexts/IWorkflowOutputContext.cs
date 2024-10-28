namespace BT.Common.WorkflowActivities.Contexts
{
    public interface IWorkflowOutputContext<TReturn>
    {
        TReturn? ReturnObject { get; set; }
    }
}
