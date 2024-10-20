namespace BT.Common.Workflow.Activities.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DefaultActivityRetryAttribute : Attribute
    {
        public int RetryCount { get; init; }
        public int SecondsBetweenRetries { get; init; }

        public DefaultActivityRetryAttribute(int retryCount, int secondsBetweenRetries)
        {
            RetryCount = retryCount;
            SecondsBetweenRetries = secondsBetweenRetries;
        }
    }
}
