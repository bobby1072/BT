using System.Data.Common;
using BT.Common.Persistence.Shared.Configurations;
using BT.Common.Persistence.Shared.Migrations.Abstract;
using BT.Common.Persistence.Shared.Migrations.Concrete;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BT.Common.Persistence.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds database migrators to the service collection and configures the necessary health checks and hosted
    /// service for executing database migrations.
    /// </summary>
    /// <param name="services">The service collection to which the database migrators will be added.</param>
    /// <param name="migrationSettings">The settings for database migrations, including retry policies and other configurations.</param>
    /// <param name="healthChecksBuilder">The health checks builder to which the database migrator health check will be added.</param>
    /// <param name="migratorConnectionFactories">An array of tuples, each containing a connection factory and the corresponding SQL folder path for the migrator.</param>
    /// <returns>The updated service collection with the database migrators, health checks, and hosted service configured.</returns>
    public static IServiceCollection AddDatabaseMigrators(
        this IServiceCollection services,
        DbMigrationSettings migrationSettings,
        IHealthChecksBuilder healthChecksBuilder,
        params (
            Func<DbConnection> ConnectionFactory,
            string SqlFolderPath
        )[] migratorConnectionFactories
    )
    {
        services.AddSingleton<IDatabaseMigratorHealthCheck, DatabaseMigratorHealthCheck>();

        healthChecksBuilder.AddCheck<IDatabaseMigratorHealthCheck>(
            nameof(IDatabaseMigratorHealthCheck)
        );

        foreach (var (connectionFactory, sqlFolderPath) in migratorConnectionFactories)
        {
            services.AddSingleton<IMigrator, DatabaseMigrator>(
                serviceProvider => new DatabaseMigrator(
                    migrationSettings,
                    connectionFactory,
                    sqlFolderPath,
                    serviceProvider
                        .GetRequiredService<ILoggerFactory>()
                        .CreateLogger<DatabaseMigrator>()
                )
            );
        }
        services.AddHostedService<DatabaseMigratorHostedService>();

        return services;
    }
}
