﻿namespace BT.Common.Workflow.Activities.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DefaultActivityRetryAttribute : Attribute
    {
        public int RetryCount { get; init; }
        public int SecondsBetweenRetries { get; init; }
        public bool RetryOnException { get; init; }
        public bool RetryOnFailedActivityResult { get; init; }
        public DefaultActivityRetryAttribute(int retryCount, int secondsBetweenRetries,
            bool retryOnException = true, bool retryOnFailedActivityResult = true)
        {
            RetryCount = retryCount;
            SecondsBetweenRetries = secondsBetweenRetries;
            RetryOnException = retryOnException;
            RetryOnFailedActivityResult = retryOnFailedActivityResult;
        }
    }
}
