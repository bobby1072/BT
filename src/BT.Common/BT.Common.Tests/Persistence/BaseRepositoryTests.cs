using BT.Common.Tests.Persistence.TestSupport;

namespace BT.Common.Tests.Persistence;

public sealed class BaseRepositoryTests : IDisposable
{
    private readonly SqliteTestDatabase _database = new();
    private readonly TestRepository _repository;

    public BaseRepositoryTests()
    {
        _repository = new TestRepository(_database.ContextFactory);
    }

    public void Dispose() => _database.Dispose();

    [Fact]
    public async Task CreateAsync_Should_Persist_Entity()
    {
        // Arrange
        var model = new TestModel(Guid.NewGuid(), "first");

        // Act
        var createResult = await _repository.CreateAsync(model);
        var getResult = await _repository.GetOneAsync(model.Id);

        // Assert
        Assert.True(createResult.IsSuccessful);
        Assert.Equal(model, getResult.Data);
    }

    [Fact]
    public async Task CreateAsync_Should_Return_Unsuccessful_Result_For_Empty_Input()
    {
        // Act
        var result = await _repository.CreateAsync(Array.Empty<TestModel>());

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Empty(result.Data);
    }

    [Fact]
    public async Task GetOneAsync_Should_Return_Null_Data_When_Missing()
    {
        // Act
        var result = await _repository.GetOneAsync(Guid.NewGuid());

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_All_Entities()
    {
        // Arrange
        await SeedAsync("a", "b", "c");

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.Equal(3, result.Data.Count);
    }

    [Fact]
    public async Task GetManyAsync_By_Ids_Should_Return_Only_Requested_Entities()
    {
        // Arrange
        var models = await SeedAsync("a", "b", "c");

        // Act
        var result = await _repository.GetManyAsync(new[] { models[0].Id, models[2].Id });

        // Assert
        Assert.Equal(2, result.Data.Count);
        Assert.Contains(models[0], result.Data);
        Assert.Contains(models[2], result.Data);
    }

    [Fact]
    public async Task GetManyAsync_By_Property_Should_Return_Matching_Entities()
    {
        // Arrange
        await SeedAsync("a", "b", "a");

        // Act
        var result = await _repository.GetManyAsync(
            new Dictionary<string, object?> { ["Name"] = "a" }
        );

        // Assert
        Assert.Equal(2, result.Data.Count);
    }

    [Fact]
    public async Task GetManyAsync_By_Unknown_Property_Should_Throw()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _repository.GetManyAsync(new Dictionary<string, object?> { ["Missing"] = "x" })
        );
    }

    [Fact]
    public async Task GetOneAsync_By_Property_Should_Return_Match()
    {
        // Arrange
        await SeedAsync("a", "b");

        // Act
        var result = await _repository.GetOneAsync("b", "Name");

        // Assert
        Assert.Equal("b", result.Data?.Name);
    }

    [Fact]
    public async Task GetCountAsync_Should_Apply_Predicate()
    {
        // Arrange
        await SeedAsync("a", "b", "a");

        // Act
        var result = await _repository.GetCountAsync(x => x.Name == "a");

        // Assert
        Assert.Equal(2, result.Data);
    }

    [Fact]
    public async Task ExistsAsync_Should_Reflect_Presence()
    {
        // Arrange
        var models = await SeedAsync("a");

        // Act
        var present = await _repository.ExistsAsync(models[0].Id);
        var absent = await _repository.ExistsAsync(Guid.NewGuid());

        // Assert
        Assert.True(present.Data);
        Assert.False(absent.Data);
    }

    [Fact]
    public async Task AnyExistsAsync_Should_Return_True_When_Any_Id_Matches()
    {
        // Arrange
        var models = await SeedAsync("a");

        // Act
        var result = await _repository.AnyExistsAsync(new[] { Guid.NewGuid(), models[0].Id });

        // Assert
        Assert.True(result.Data);
    }

    [Fact]
    public async Task UpdateAsync_Should_Persist_Changes()
    {
        // Arrange
        var models = await SeedAsync("original");

        // Act
        await _repository.UpdateAsync(new TestModel(models[0].Id, "changed"));
        var result = await _repository.GetOneAsync(models[0].Id);

        // Assert
        Assert.Equal("changed", result.Data?.Name);
    }

    [Fact]
    public async Task DeleteAsync_By_Model_Should_Remove_Entity()
    {
        // Arrange
        var models = await SeedAsync("a", "b");

        // Act
        await _repository.DeleteAsync(models[0]);
        var count = await _repository.GetCountAsync();

        // Assert
        Assert.Equal(1, count.Data);
        Assert.Null((await _repository.GetOneAsync(models[0].Id)).Data);
    }

    [Fact]
    public async Task DeleteAsync_By_Ids_Should_Remove_Only_Requested_Entities()
    {
        // Arrange
        var models = await SeedAsync("a", "b");

        // Act
        var result = await _repository.DeleteAsync(new[] { models[0].Id });
        var remaining = await _repository.GetAllAsync();

        // Assert
        Assert.Equal(new[] { models[0].Id }, result.Data);
        Assert.Equal(models[1], Assert.Single(remaining.Data));
    }

    private async Task<TestModel[]> SeedAsync(params string[] names)
    {
        var models = names.Select(name => new TestModel(Guid.NewGuid(), name)).ToArray();
        await _repository.CreateAsync(models);
        return models;
    }
}
