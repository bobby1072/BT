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
    public static IHostBuilder CreateDefaultEvolveDbUpMigratorHostBuilder<TProgram>(
        string[] args,
        Func<IConfiguration, string> getConnectionStringFunc,
        Func<IConfiguration, DbMigrationSettings> getDbMigrationsSettingsFunc,
        string serviceName,
        bool shutDownAppAfterMigrations = true,
        params (
            Func<DbConnection> ConnectionFactory,
            string SqlFolderPath
        )[] migratorConnectionFactories
    )
        where TProgram : class
    {
        var localLogger = LoggingHelper.CreateLogger();

        localLogger.LogInformation("Application starting...");

        var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(config =>
            {
                config
                    .SetBasePath(Environment.CurrentDirectory)
                    .AddJsonFile(Path.GetFullPath("appsettings.json"), false)
                    .AddUserSecrets<TProgram>()
                    .AddEnvironmentVariables();
            })
            .ConfigureEvolveDbUp(
                getConnectionStringFunc,
                getDbMigrationsSettingsFunc,
                serviceName,
                shutDownAppAfterMigrations,
                migratorConnectionFactories
            );

        return host;
    }

    public static IHostBuilder CreateDefaultEvolveDbUpMigratorHostBuilder(
        string[] args,
        Func<IConfiguration, string> getConnectionStringFunc,
        Func<IConfiguration, DbMigrationSettings> getDbMigrationsSettingsFunc,
        string serviceName,
        bool shutDownAppAfterMigrations = true,
        params (
            Func<DbConnection> ConnectionFactory,
            string SqlFolderPath
        )[] migratorConnectionFactories
    )
    {
        var localLogger = LoggingHelper.CreateLogger();

        localLogger.LogInformation("Application starting...");

        var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(config =>
            {
                config
                    .SetBasePath(Environment.CurrentDirectory)
                    .AddJsonFile(Path.GetFullPath("appsettings.json"), false)
                    .AddEnvironmentVariables();
            })
            .ConfigureEvolveDbUp(
                getConnectionStringFunc,
                getDbMigrationsSettingsFunc,
                serviceName,
                shutDownAppAfterMigrations,
                migratorConnectionFactories
            );

        return host;
    }

    private static IHostBuilder ConfigureEvolveDbUp(
        this IHostBuilder host,
        Func<IConfiguration, string> getConnectionStringFunc,
        Func<IConfiguration, DbMigrationSettings> getDbMigrationsSettingsFunc,
        string serviceName,
        bool shutDownAppAfterMigrations,
        params (
            Func<DbConnection> ConnectionFactory,
            string SqlFolderPath
        )[] migratorConnectionFactories
    )
    {
        host.ConfigureLogging(lgBuilder =>
            {
                lgBuilder.AddJsonLogging();
            })
            .ConfigureServices(
                (ctx, serviceCol) =>
                {
                    serviceCol.AddTelemetryServices(serviceName);

                    var dbMigrationsSettings = getDbMigrationsSettingsFunc.Invoke(
                        ctx.Configuration
                    );

                    var connectionString = getConnectionStringFunc.Invoke(ctx.Configuration);

                    if (string.IsNullOrWhiteSpace(connectionString))
                    {
                        throw new InvalidDataException("Connection string string not found");
                    }
                    var healthCheckBuilder = serviceCol.AddHealthChecks();

                    serviceCol.AddDatabaseMigrators(
                        dbMigrationsSettings,
                        healthCheckBuilder,
                        shutDownAppAfterMigrations,
                        migratorConnectionFactories
                    );
                }
            );

        return host;
    }
}
