using BT.Common.Persistence.Shared.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;

namespace BT.Common.Tests.Persistence.TestSupport;

public sealed class TestRepository(IDbContextFactory<TestDbContext> contextFactory)
    : BaseRepository<TestEntity, Guid, TestModel, TestDbContext>(
        contextFactory,
        NullLogger<BaseRepository<TestEntity, Guid, TestModel, TestDbContext>>.Instance
    )
{
    protected override TestEntity RuntimeToEntity(TestModel runtimeObj) =>
        TestEntity.FromModel(runtimeObj);
}

public sealed class TestCacheRepository(
    IDbContextFactory<TestDbContext> contextFactory,
    IMemoryCache memoryCache,
    TimeSpan timeToCache
) : BaseCacheRepository<TestEntity, Guid, TestModel, TestDbContext>(
        contextFactory,
        memoryCache,
        timeToCache,
        NullLogger<BaseRepository<TestEntity, Guid, TestModel, TestDbContext>>.Instance
    )
{
    protected override TestEntity RuntimeToEntity(TestModel runtimeObj) =>
        TestEntity.FromModel(runtimeObj);
}
