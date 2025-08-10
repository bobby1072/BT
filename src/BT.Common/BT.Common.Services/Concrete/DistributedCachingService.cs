using System.Text.Json;
using BT.Common.Services.Abstract;
using BT.Common.Services.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace BT.Common.Services.Concrete;

internal sealed class DistributedCachingService : ICachingService
{
    private static readonly Type _typeofString = typeof(string);
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<DistributedCachingService> _logger;

    public DistributedCachingService(
        IDistributedCache distributedCache,
        ILogger<DistributedCachingService> logger
    )
    {
        _distributedCache = distributedCache;
        _logger = logger;
    }

    public async Task<bool> TryRemoveObjectAsync(string key)
    {
        try
        {
            await _distributedCache.RemoveAsync(key);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception occurred removing object from cache");

            return false;
        }
    }

    public async Task<T?> TryGetObjectAsync<T>(string key)
        where T : class
    {
        try
        {
            return await GetObject<T>(key);
        }
        catch (KeyNotFoundException e)
        {
            _logger.LogInformation(e, "Couldn't find object in cache");

            return null;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception occurred getting object from cache");

            return null;
        }
    }

    public Task<string> SetObjectAsync<T>(
        string key,
        T value,
        CacheObjectTimeToLiveInSeconds timeToLive = CacheObjectTimeToLiveInSeconds.TenMinutes
    )
        where T : class
    {
        return SetObjectAsync(
            key,
            value,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds((int)timeToLive),
            }
        );
    }

    public async Task<string> SetObjectAsync<T>(
        string key,
        T value,
        DistributedCacheEntryOptions options
    )
        where T : class
    {
        if (typeof(T) == _typeofString)
        {
            _logger.LogDebug("Attempting to cache {StringValue}", value);

            await _distributedCache.SetStringAsync(key, (value as string)!, options);
        }
        else
        {

            _logger.LogDebug("Attempting to cache {@ObjToCache}", value);

            var serializedValue = JsonSerializer.Serialize(value);
            
            await _distributedCache.SetStringAsync(key, serializedValue, options);
        }
        return key;
    }

    private async Task<T> GetObject<T>(string key)
        where T : class
    {
        var foundValue =
            await _distributedCache.GetStringAsync(key)
            ?? throw new KeyNotFoundException("Cannot find object with that key");
        if (typeof(T) == _typeofString)
        {
            return foundValue as T ?? throw new InvalidDataException("Cannot parse object");
        }
        return JsonSerializer.Deserialize<T>(foundValue)
            ?? throw new InvalidDataException("Cannot parse object");
    }
}


