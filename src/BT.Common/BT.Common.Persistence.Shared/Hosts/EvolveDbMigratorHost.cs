using System.Data.Common;
using BT.Common.Helpers;
using BT.Common.Persistence.Shared.Configurations;
using BT.Common.Persistence.Shared.Extensions;
using BT.Common.Services.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BT.Common.Persistence.Shared.Hosts;

public static class EvolveDbMigratorHost
{
    public static HostApplicationBuilder CreateDefaultEvolveDbUpMigratorHostBuilder<TProgram>(
        string[] args,
        Func<IConfiguration, string> getConnectionStringFunc,
        Func<IConfiguration, DbMigrationSettings> getDbMigrationsSettingsFunc,
        string serviceName,
        bool shutDownAppAfterMigrations = true,
        params (
            Func<string, DbConnection> ConnectionFactory,
            string SqlFolderPath
        )[] migratorConnectionFactories
    )
        where TProgram : class
    {
        var localLogger = LoggingHelper.CreateLogger();

        localLogger.LogInformation("Application starting...");

        var builder = Host.CreateApplicationBuilder(args);
        
        builder.Configuration
            .SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile(Path.GetFullPath("appsettings.json"), false)
            .AddUserSecrets<TProgram>()
            .AddEnvironmentVariables();
        
            
        builder
            .ConfigureEvolveDbUp(
                getConnectionStringFunc,
                getDbMigrationsSettingsFunc,
                serviceName,
                shutDownAppAfterMigrations,
                migratorConnectionFactories
            );
        
        
        return builder;
    }

    public static HostApplicationBuilder CreateDefaultEvolveDbUpMigratorHostBuilder(
        string[] args,
        Func<IConfiguration, string> getConnectionStringFunc,
        Func<IConfiguration, DbMigrationSettings> getDbMigrationsSettingsFunc,
        string serviceName,
        bool shutDownAppAfterMigrations = true,
        params (
            Func<string, DbConnection> ConnectionFactory,
            string SqlFolderPath
        )[] migratorConnectionFactories
    )
    {
        var localLogger = LoggingHelper.CreateLogger();

        localLogger.LogInformation("Application starting...");

        var builder = Host.CreateApplicationBuilder(args);
        
        builder.Configuration
            .SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile(Path.GetFullPath("appsettings.json"), false)
            .AddEnvironmentVariables();
        
            
        builder
            .ConfigureEvolveDbUp(
                getConnectionStringFunc,
                getDbMigrationsSettingsFunc,
                serviceName,
                shutDownAppAfterMigrations,
                migratorConnectionFactories
            );

        return builder;
    }

    private static HostApplicationBuilder ConfigureEvolveDbUp(
        this HostApplicationBuilder builder,
        Func<IConfiguration, string> getConnectionStringFunc,
        Func<IConfiguration, DbMigrationSettings> getDbMigrationsSettingsFunc,
        string serviceName,
        bool shutDownAppAfterMigrations,
        params (
            Func<string, DbConnection> ConnectionFactory,
            string SqlFolderPath
        )[] migratorConnectionFactories
    )
    {
        builder.Logging.AddJsonLogging();
            
        builder.Services.AddTelemetryServices(serviceName);

        var dbMigrationsSettings = getDbMigrationsSettingsFunc.Invoke(
            builder.Configuration
        );

        var connectionString = getConnectionStringFunc.Invoke(builder.Configuration);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidDataException("Connection string string not found");
        }
        var healthCheckBuilder = builder.Services.AddHealthChecks();

        builder.Services.AddDatabaseMigrators(
            dbMigrationsSettings,
            healthCheckBuilder,
            shutDownAppAfterMigrations,
            migratorConnectionFactories.Select(x => BuildConnection(x, connectionString)).ToArray()   
        );

        return builder;
    }

    private static (Func<DbConnection> ConnectionFactory, string SqlFolderPath) BuildConnection((
        Func<string, DbConnection> ConnectionFactory,
        string SqlFolderPath
        ) migratorConnectionFactory,
        string connectionString
    ) => (() => migratorConnectionFactory.ConnectionFactory.Invoke(connectionString),
            migratorConnectionFactory.SqlFolderPath);
}
