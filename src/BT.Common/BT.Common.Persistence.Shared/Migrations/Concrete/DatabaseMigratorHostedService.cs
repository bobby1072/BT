using BT.Common.Persistence.Shared.Configurations;
using BT.Common.Persistence.Shared.Migrations.Abstract;
using BT.Common.Polly.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BT.Common.Persistence.Shared.Migrations.Concrete
{
    internal sealed class DatabaseMigratorHostedService : BackgroundService
    {
        private readonly IEnumerable<IMigrator> _databaseMigrators;
        private readonly DbMigrationSettings _dbMigrationsConfiguration;
        private readonly IDatabaseMigratorHealthCheck _databaseMigratorHealthCheck;
        private readonly ILogger<DatabaseMigratorHostedService> _logger;

        public DatabaseMigratorHostedService(
            IEnumerable<IMigrator>? databaseMigrators,
            DbMigrationSettings dbMigrationsConfiguration,
            IDatabaseMigratorHealthCheck databaseMigratorHealthCheck,
            ILogger<DatabaseMigratorHostedService> logger
        )
        {
            _databaseMigrators = databaseMigrators ?? new List<IMigrator>();
            _dbMigrationsConfiguration = dbMigrationsConfiguration;
            _databaseMigratorHealthCheck = databaseMigratorHealthCheck;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting database migrations at {MigrationTime}...",
                DateTime.UtcNow);
            
            var pipeline = _dbMigrationsConfiguration.ToPipeline();

            await pipeline.ExecuteAsync(async _ => await Migrate(), cancellationToken);

            _databaseMigratorHealthCheck.SetMigrationCompleted(true);
            _logger.LogInformation("Migrations have been successfully completed...");
        }

        private async Task Migrate()
        {
            foreach (var migrator in _databaseMigrators)
            {
                await migrator.Migrate();
            }
        }
    }
}
