using System.Data.Common;
using BT.Common.Persistence.Shared.Configurations;
using BT.Common.Persistence.Shared.Migrations.Abstract;
using BT.Common.Persistence.Shared.Migrations.Concrete;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BT.Common.Persistence.Shared.Extensions;

public static class ServiceCollectionExtensions
{
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
