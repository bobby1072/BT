namespace BT.Common.Workflow.Contexts
{
    public interface IWorkflowOutputContext<TReturn>
    {
        TReturn? ReturnObject { get; set; }
    }
}
