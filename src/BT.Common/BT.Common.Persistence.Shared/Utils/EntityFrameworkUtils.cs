using BT.Common.Persistence.Shared.Models;
using Microsoft.Extensions.Logging;

namespace BT.Common.Persistence.Shared.Utils
{
    public static class EntityFrameworkUtils
    {
        public static async Task TryDbOperation(
            Func<Task> dbOperation,
            ILogger<object>? logger = null
        )
        {
            try
            {
                await dbOperation.Invoke();
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
            ILogger<object>? logger = null
        )
            where TResult : DbResult
        {
            try
            {
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
            ILogger<object>? logger = null
        )
            where TResult : DbResult
        {
            try
            {
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
