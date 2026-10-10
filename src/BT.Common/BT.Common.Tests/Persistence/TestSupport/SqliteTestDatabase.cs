using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace BT.Common.Tests.Persistence.TestSupport;

public sealed class SqliteTestDatabase : IDisposable
{
    // Keeps the shared in-memory database alive for the lifetime of the test.
    private readonly SqliteConnection _keepAliveConnection;
    private readonly ServiceProvider _serviceProvider;

    public SqliteTestDatabase()
    {
        var connectionString = $"DataSource=test-{Guid.NewGuid():N};Mode=Memory;Cache=Shared";
        _keepAliveConnection = new SqliteConnection(connectionString);
        _keepAliveConnection.Open();

        var services = new ServiceCollection();
        services.AddMemoryCache();
        services.AddDbContextFactory<TestDbContext>(options => options.UseSqlite(connectionString));
        _serviceProvider = services.BuildServiceProvider();

        using var context = ContextFactory.CreateDbContext();
        context.Database.EnsureCreated();
    }

    public IDbContextFactory<TestDbContext> ContextFactory =>
        _serviceProvider.GetRequiredService<IDbContextFactory<TestDbContext>>();

    public IMemoryCache MemoryCache => _serviceProvider.GetRequiredService<IMemoryCache>();

    public void Dispose()
    {
        _serviceProvider.Dispose();
        _keepAliveConnection.Dispose();
    }
}
