namespace BT.Common.Workflow.Exceptions
{
    public class WorkflowException : Exception
    {
        public WorkflowException(string message)
            : base(message) { }

        public WorkflowException(string message, Exception innerException) : base(message, innerException) { }
    }
}
