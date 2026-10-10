using BT.Common.Tests.Persistence.TestSupport;
using Microsoft.EntityFrameworkCore;

namespace BT.Common.Tests.Persistence;

public sealed class BaseCacheRepositoryTests : IDisposable
{
    private readonly SqliteTestDatabase _database = new();
    private readonly TestCacheRepository _repository;

    public BaseCacheRepositoryTests()
    {
        _repository = new TestCacheRepository(
            _database.ContextFactory,
            _database.MemoryCache,
            TimeSpan.FromMinutes(5)
        );
    }

    public void Dispose() => _database.Dispose();

    [Fact]
    public async Task GetOneAsync_Should_Serve_Repeat_Reads_From_Cache()
    {
        // Arrange
        var model = new TestModel(Guid.NewGuid(), "original");
        await _repository.CreateAsync(model);
        await _repository.GetOneAsync(model.Id);
        await using (var context = await _database.ContextFactory.CreateDbContextAsync())
        {
            // Bypasses the repository so the cached value is the only source of the original name.
            await context.TestEntities
                .Where(x => x.Id == model.Id)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.Name, "changed"));
        }

        // Act
        var result = await _repository.GetOneAsync(model.Id);

        // Assert
        Assert.Equal("original", result.Data?.Name);
    }

    [Fact]
    public async Task UpdateAsync_Should_Invalidate_Cached_Entity()
    {
        // Arrange
        var model = new TestModel(Guid.NewGuid(), "original");
        await _repository.CreateAsync(model);
        await _repository.GetOneAsync(model.Id);

        // Act
        await _repository.UpdateAsync(model with { Name = "changed" });
        var result = await _repository.GetOneAsync(model.Id);

        // Assert
        Assert.Equal("changed", result.Data?.Name);
    }

    [Fact]
    public async Task GetManyAsync_By_Ids_Should_Return_Entities_On_Cold_Cache()
    {
        // Arrange
        var models = new[]
        {
            new TestModel(Guid.NewGuid(), "a"),
            new TestModel(Guid.NewGuid(), "b"),
        };
        await _repository.CreateAsync(models);

        // Act
        var result = await _repository.GetManyAsync(models.Select(x => x.Id).ToArray());

        // Assert
        Assert.Equal(2, result.Data.Count);
    }
}
