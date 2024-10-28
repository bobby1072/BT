namespace BT.Common.WorkflowActivities
{
    internal static class WorkflowConstants
    {
        public const string CouldNotResolveWorkflow =
            $"Could not resolve workflow from {nameof(IServiceProvider)}";
        public const string CouldNotResolveActivity =
            $"Could not resolve activity from {nameof(IServiceProvider)}";

        public const string CouldNotGetResultFromActivity =
            "Could not get activity to return a result";
    }
}
