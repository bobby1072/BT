namespace BT.Common.Workflow.Common
{
    internal static class WorkflowConstants
    {
        public const string CouldNotResolveActivity =
            $"Could not resolve activity from {nameof(IServiceProvider)}";

        public const string CouldNotGetResultFromActivity =
            "Could not get activity to return a result";
    }
}
