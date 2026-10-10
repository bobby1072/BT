using BT.Common.Persistence.Shared.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BT.Common.Tests.Persistence.TestSupport;

public sealed class TestDbContext(IMemoryCache memoryCache, DbContextOptions<TestDbContext> options)
    : BaseCacheDbContext(memoryCache, options)
{
    public DbSet<TestEntity> TestEntities { get; set; } = null!;
}
