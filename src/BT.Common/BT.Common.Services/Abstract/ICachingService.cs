using BT.Common.Services.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace BT.Common.Services.Abstract;


public interface ICachingService
{
    Task<T?> TryGetObjectAsync<T>(string key, CancellationToken cancellationToken = default)
        where T : class;
    Task<string> SetObjectAsync<T>(
        string key,
        T value,
        CacheObjectTimeToLiveInSeconds timeToLive = CacheObjectTimeToLiveInSeconds.TenMinutes, CancellationToken cancellationToken = default
    )
        where T : class;
    Task<string> SetObjectAsync<T>(string key, T value, DistributedCacheEntryOptions options, CancellationToken cancellationToken = default)
        where T : class;
}
