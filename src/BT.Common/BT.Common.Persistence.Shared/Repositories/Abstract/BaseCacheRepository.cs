using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using BT.Common.Persistence.Shared.Attributes;
using BT.Common.Persistence.Shared.Contexts;
using BT.Common.Persistence.Shared.Entities;
using BT.Common.Persistence.Shared.Models;
using BT.Common.Polly.Models.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace BT.Common.Persistence.Shared.Repositories.Abstract;

public abstract class BaseCacheRepository<TEnt, TEntId, TModel, TDbContext>
    : BaseRepository<TEnt, TEntId, TModel, TDbContext>
    where TEnt : BaseEntity<TEntId, TModel>
    where TModel : class
    where TDbContext : BaseCacheDbContext
{
    protected static Type TModelType = typeof(TModel);
    private readonly IMemoryCache _memoryCache;

    protected BaseCacheRepository(
        IDbContextFactory<TDbContext> dbContextFactory,
        IMemoryCache memoryCache,
        ILogger<BaseRepository<TEnt, TEntId, TModel, TDbContext>> logger,
        IPollyRetrySettings? pollyRetrySettings = null
    ) : base(dbContextFactory, logger, pollyRetrySettings)
    {
        _memoryCache = memoryCache;
    }
    public override async Task<DbGetManyResult<TModel>> GetManyAsync(
        IReadOnlyCollection<TEntId> entityIds,
        CancellationToken cancellationToken = default,
        params string[] relations
    )
    {
        var cachedList = new List<TModel>();
        var nonFoundCacheIds = entityIds.Where(x =>
        {
            var foundCachedEntity = GetItemFromCache(x!.ToString()!, relations);
            if (foundCachedEntity is not null)
            {
                cachedList.Add(foundCachedEntity.Data!);
            }

            return foundCachedEntity is null;
        }).ToArray();

        if (nonFoundCacheIds.Length != entityIds.Count)
        {
            var foundFromDb = await base.GetManyAsync(nonFoundCacheIds, cancellationToken, relations);
            
            CacheResultIfPossible((IReadOnlyCollection<DbGetOneResult<TModel>>)foundFromDb.Data);
            
            return new DbGetManyResult<TModel>(cachedList.Concat(foundFromDb.Data).ToArray());
        }
        return new DbGetManyResult<TModel>(cachedList);
    }

    public override async Task<DbGetOneResult<TModel>> GetOneAsync(
        TEntId entityId,
        CancellationToken cancellationToken = default,
        params string[] relations
    )
    {
        var foundCachedObject = GetItemFromCache(entityId!.ToString()!, relations);

        if (foundCachedObject is not null)
        {
            return foundCachedObject;
        }
        var result  = await base.GetOneAsync(entityId, cancellationToken, relations);
        
        CacheResultIfPossible(result);
        
        return result;
    }

    public override async Task<DbGetOneResult<TModel>> GetOneAsync(
        Dictionary<string, object?> propertiesToMatch,
        CancellationToken cancellationToken = default,
        params string[] relations
    )
    {
        var result  = await base.GetOneAsync(propertiesToMatch, cancellationToken, relations);
        
        CacheResultIfPossible(result);
        
        return result;
    }
    /// <summary>
    /// WARNING: this method is dangerous as it can cache something then later when a new entity is added which also satifies the condition. It still returns the original.
    /// Should only be used for deterministic queries, like on enforced unique properties.
    /// </summary>
    public override async Task<DbGetOneResult<TModel>> GetOneAsync<T>(
        T value,
        string propertyName,
        CancellationToken cancellationToken = default,
        params string[] relations
    )
    {
        var foundCachedObject = GetItemFromCache($"{propertyName}_{value?.ToString() ?? TModelType.FullName}", relations);

        if (foundCachedObject is not null)
        {
            return foundCachedObject;
        }
        
        var result  = await base.GetOneAsync(value, propertyName, cancellationToken, relations);
        
        CacheResultIfPossible(result);
        CacheResultIfPossible(value, propertyName, result);
        
        return result;
    }

    private DbGetOneResult<TModel>? GetItemFromCache(string objectIdOrQueryParams, string[] relations)
    {
        if (EntityType.GetCustomAttribute<CacheableAttribute>() is null)
        {
            return null;
        }
        
        var foundCachedObject = _memoryCache.Get<DbGetOneResult<TModel>>(GetCacheKey(objectIdOrQueryParams));

        if (relations.Length > 0 &&  foundCachedObject?.Data is not null)
        {
            return relations.All(x => GetValueFromProperty(foundCachedObject.Data, x) is not null) ?  new DbGetOneResult<TModel>(foundCachedObject.Data) : null;
        }
        
        return foundCachedObject;
    }
    private void CacheResultIfPossible<T>(T value,
        string propertyName, 
        DbGetOneResult<TModel> result)
    {
        if (result.Data is not null && EntityType.GetCustomAttribute<CacheableAttribute>() is not null)
        {
            _memoryCache.Set(GetCacheKey($"{propertyName}_{value?.ToString() ?? TModelType.FullName}"), result, new MemoryCacheEntryOptions().SetSize(1));
        }
    }
    private void CacheResultIfPossible(DbGetOneResult<TModel> result)
    {
        if (result.Data is not null && EntityType.GetCustomAttribute<CacheableAttribute>() is not null)
        {
            var foundResultId = GetIdFromEntity(result.Data);
            if (foundResultId is not null)
            {
                _memoryCache.Set(GetCacheKey(foundResultId), result, new MemoryCacheEntryOptions().SetSize(1));
            }
        }
    }
    private void CacheResultIfPossible(IReadOnlyCollection<DbGetOneResult<TModel>> result)
    {
        foreach (var item in result)
        {
            CacheResultIfPossible(item);
        }
    }
    private static string GetCacheKey(string objectId) => $"{EntityType.FullName}__{objectId}";
    private static string? GetIdFromEntity(TModel value)
    {
        var foundIdProperty =  TModelType.GetProperty("Id");

        return foundIdProperty?.GetValue(value)?.ToString();
    }

    private static object? GetValueFromProperty(TModel value, string propName) => TModelType.GetProperty(propName)?.GetValue(value);
}