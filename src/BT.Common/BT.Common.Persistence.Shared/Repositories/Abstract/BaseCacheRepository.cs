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
    where TDbContext : DbContext
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
    
    public override async Task<DbGetManyResult<TModel>> GetManyAsync<T>(
        T value,
        string propertyName,
        CancellationToken cancellationToken = default,
        params string[] relations
    );
    public override async Task<DbGetManyResult<TModel>> GetManyAsync(
        Dictionary<string, object?> propertiesToMatch,
        CancellationToken cancellationToken = default,
        params string[] relations
    );
    public override async Task<DbGetManyResult<TModel>> GetManyAsync(
        TEntId entityId,
        CancellationToken cancellationToken = default,
        params string[] relations
    );

    public override async Task<DbGetManyResult<TModel>> GetManyAsync(
        IReadOnlyCollection<TEntId> entityIds,
        CancellationToken cancellationToken = default,
        params string[] relations
    );

    public override async Task<DbGetOneResult<TModel>> GetOneAsync(
        TEntId entityId,
        CancellationToken cancellationToken = default,
        params string[] relations
    )
    {
        var foundCachedObject = GetItemFromCache(entityId!.ToString()!);

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
        var combinedQueryParams = string.Join("__",propertiesToMatch.Select(x => $"{x.Key}_{x.Value}"));
        
        var foundCachedObject = GetItemFromCache(combinedQueryParams);

        if (foundCachedObject is not null)
        {
            return foundCachedObject;
        }
        
        var result  = await base.GetOneAsync(propertiesToMatch, cancellationToken, relations);
        
        CacheResultIfPossible(result, combinedQueryParams);
        
        return result;
    }

    public override async Task<DbGetOneResult<TModel>> GetOneAsync<T>(
        T value,
        string propertyName,
        CancellationToken cancellationToken = default,
        params string[] relations
    )
    {
        var combinedQueryParams = $"{propertyName}_{value?.ToString()}";
        var foundCachedObject = GetItemFromCache(combinedQueryParams);

        if (foundCachedObject is not null)
        {
            return foundCachedObject;
        }
        
        var result  = await base.GetOneAsync(value, propertyName, cancellationToken, relations);
        
        CacheResultIfPossible(result, combinedQueryParams);
        
        return result;
    }

    private DbGetOneResult<TModel>? GetItemFromCache(string objectIdOrQueryParams)
    {
        var foundCachedObject = _memoryCache.Get<DbGetOneResult<TModel>>(GetCacheKey(objectIdOrQueryParams));

        return foundCachedObject;
    }
    private void CacheResultIfPossible(DbGetOneResult<TModel> result, string? combinedQueryParams = null)
    {
        if (result.Data is not null)
        {
            var foundResultId = GetIdFromEntity(result.Data);
            if (foundResultId is not null)
            {
                _memoryCache.Set(GetCacheKey(foundResultId), result);
            }
        }
        if (!string.IsNullOrWhiteSpace(combinedQueryParams))
        {
            _memoryCache.Set(GetCacheKey(combinedQueryParams), result);
        }
    }
    private static string GetCacheKey(string objectIdOrQueryParams) => $"{EntityType.FullName}_{objectIdOrQueryParams}";
    private static string? GetIdFromEntity(TModel value)
    {
        var foundIdProperty =  TModelType.GetProperty("Id");

        return foundIdProperty?.GetValue(value)?.ToString();
    }
}