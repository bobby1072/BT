using BT.Common.Services.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace BT.Common.Services.Abstract;


public interface ICachingService
{
    Task<T?> TryGetObjectAsync<T>(string key)
        where T : class;
    Task<string> SetObjectAsync<T>(
        string key,
        T value,
        CacheObjectTimeToLiveInSeconds timeToLive = CacheObjectTimeToLiveInSeconds.TenMinutes
    )
        where T : class;
    Task<string> SetObjectAsync<T>(string key, T value, DistributedCacheEntryOptions options)
        where T : class;
}
