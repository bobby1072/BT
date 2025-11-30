using BT.Common.Persistence.Shared.Models;
using BT.Common.Polly.Extensions;
using BT.Common.Polly.Models.Abstract;
using Microsoft.Extensions.Logging;

namespace BT.Common.Persistence.Shared.Utils
{
    public static class EntityFrameworkUtils
    {
        public static async Task TryDbOperation(
            Func<Task> dbOperation,
            ILogger<object>? logger = null,
            IPollyRetrySettings? pollyRetry = null
        )
        {
            try
            {
                if (pollyRetry is not null)
                {
                    var retryPipe = pollyRetry.ToPipeline();
                    
                    await retryPipe.ExecuteAsync(async _ =>  await dbOperation.Invoke());
                }
                else
                {
                    await dbOperation.Invoke();
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(
                    ex,
                    "Exception occurred during db operation with message {Message}",
                    ex.Message
                );
            }
        }

        public static async Task<TResult?> TryDbOperation<TResult>(
            Func<Task<TResult>> dbOperation,
            ILogger<object>? logger = null,
            IPollyRetrySettings? pollyRetry = null
        )
            where TResult : DbResult
        {
            try
            {
                if (pollyRetry is not null)
                {
                    var retryPipe = pollyRetry.ToPipeline();
                    
                    return await retryPipe.ExecuteAsync(async _ =>  await dbOperation.Invoke());
                }
                return await dbOperation.Invoke();
            }
            catch (Exception ex)
            {
                logger?.LogError(
                    ex,
                    "Exception occurred during db operation with message {Message}",
                    ex.Message
                );

                return null;
            }
        }

        public static TResult? TryDbOperation<TResult>(
            Func<TResult> dbOperation,
            ILogger<object>? logger = null,
            IPollyRetrySettings? pollyRetry = null
        )
            where TResult : DbResult
        {
            try
            {
                if (pollyRetry is not null)
                {
                    var retryPipe = pollyRetry.ToPipeline();
                    
                    return retryPipe.Execute(_ => dbOperation.Invoke());
                }
                return dbOperation.Invoke();
            }
            catch (Exception ex)
            {
                logger?.LogError(
                    ex,
                    "Exception occurred during db operation with message {Message}",
                    ex.Message
                );

                return null;
            }
        }
    }
}
