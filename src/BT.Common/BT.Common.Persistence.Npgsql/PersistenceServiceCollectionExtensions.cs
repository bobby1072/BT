using BT.Common.Persistence.Npgsql.Migrations.Abstract;
using BT.Common.Persistence.Npgsql.Migrations.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace BT.Common.Persistence.Npgsql
{
    public static class PersistenceServiceCollectionExtensions
    {
        public static IServiceCollection AddSqlPersistence<TContext>(
            this IServiceCollection services,
            IConfiguration configuration,
            string? migrationStartLocation = null
        ) where TContext : DbContext
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var migrationStartVersion = configuration
                .GetSection("Migration")
                .GetSection("StartVersion")
                ?.Value;

            if (
                string.IsNullOrEmpty(connectionString)
            )
            {
                throw new InvalidDataException("Missing connection string");
            }

            if (!string.IsNullOrEmpty(migrationStartLocation) && string.IsNullOrEmpty(migrationStartVersion))
            {
                throw new InvalidDataException("Missing migration start version.");
            }
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString);


            if (migrationStartVersion != null)
            {
                services.AddSingleton<IMigrator, DatabaseMigrations>(sp => new DatabaseMigrations(
                    sp.GetRequiredService<ILoggerFactory>().CreateLogger<DatabaseMigrations>(),
                    connectionString,
                    migrationStartVersion
                ));

                services
                    .AddHostedService<DatabaseMigratorHostedService>()
                    .AddSingleton<DatabaseMigratorHealthCheck>()
                    .AddHealthChecks()
                    .AddCheck<DatabaseMigratorHealthCheck>(
                        DatabaseMigratorHealthCheck.Name,
                        tags: ["Ready"]
                    );
            }
            services
                .AddPooledDbContextFactory<TContext>(options =>
                    options
                        .UseSnakeCaseNamingConvention()
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                        .UseNpgsql(
                            connectionStringBuilder.ConnectionString,
                            options =>
                            {
                                options.UseQuerySplittingBehavior(
                                    QuerySplittingBehavior.SingleQuery
                                );
                            }
                        )
                )
                .AddHealthChecks();

            return services;
        }
    }
}
