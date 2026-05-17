using System.Reflection;
using BT.Common.Persistence.Shared.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BT.Common.Persistence.Shared.Contexts;

public abstract class BaseCacheDbContext : DbContext
{
    private readonly IMemoryCache _memoryCache;
    protected BaseCacheDbContext(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateCacheIfNeeded();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        UpdateCacheIfNeeded();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override int SaveChanges()
    {
        UpdateCacheIfNeeded();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        UpdateCacheIfNeeded();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    private void UpdateCacheIfNeeded()
    {
        var updatingEntries = ChangeTracker
            .Entries()
            .Where(e => e.State is EntityState.Deleted or EntityState.Modified)
            .ToArray();

        foreach (var ent in updatingEntries)
        {
            var foundCacheKey = GetCacheKeyFromEntity(ent.Entity);
            
            if (!string.IsNullOrWhiteSpace(foundCacheKey))
            {
                _memoryCache.Remove(foundCacheKey);
            }
        }
    }
    
    private static string? GetCacheKeyFromEntity<T>(T value)
    {
        var typeofT = typeof(T);

        if (typeofT.GetCustomAttribute<CacheableAttribute>() is null)
        {
            return  null;
        }
        
        var foundIdProperty =  
            typeofT.GetProperty("Id")?.GetValue(value)?.ToString();

        if (!string.IsNullOrWhiteSpace(foundIdProperty))
        {
            return $"{typeofT.FullName}_{foundIdProperty}";
        }

        return null;
    }
}