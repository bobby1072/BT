namespace BT.Common.WorkflowActivities.Activities.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DefaultActivityRetryAttribute : Attribute
    {
        public int RetryCount { get; init; }
        public int SecondsBetweenRetries { get; init; }
        public bool RetryOnException { get; init; }
        public DefaultActivityRetryAttribute(int retryCount, int secondsBetweenRetries,
            bool retryOnException = true)
        {
            RetryCount = retryCount;
            SecondsBetweenRetries = secondsBetweenRetries;
            RetryOnException = retryOnException;
        }
    }
}
