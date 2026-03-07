using System.Data.Common;
using BT.Common.Persistence.Shared.Configurations;
using BT.Common.Persistence.Shared.Migrations.Abstract;
using EvolveDb;
using EvolveDb.Migration;
using Microsoft.Extensions.Logging;

namespace BT.Common.Persistence.Shared.Migrations.Concrete
{
    /// <summary>
    /// Migrator class that uses Evolve to perform database migrations. It is designed to be used as a hosted service, allowing it to run in the background and perform migrations when the application starts. The migrator checks for pending migrations and applies them in order, ensuring that the database schema is up to date with the application's requirements. It also integrates with health checks to provide feedback on the migration status.
    /// </summary>
    /// <remarks>
    /// This class is intended to be used in conjunction with the <see cref="DatabaseMigratorHostedService"/> and <see cref="IDatabaseMigratorHealthCheck"/> to manage database migrations in a robust and reliable manner. It leverages the Evolve library for migration management, which supports various database providers and offers features such as versioning, out-of-order migrations, and more.
    /// </remarks>
    public sealed class DatabaseMigrator : IMigrator
    {
        private readonly Func<DbConnection> _connectionFactory;
        private readonly string _startVersion;
        private readonly string _sqlFolderPath;
        private readonly ILogger<DatabaseMigrator> _logger;

        public DatabaseMigrator(
            DbMigrationSettings migrationSettings,
            Func<DbConnection> connectionFactory,
            string sqlFolderPath,
            ILogger<DatabaseMigrator> logger
        )
        {
            _connectionFactory = connectionFactory;
            _startVersion = migrationSettings.StartVersion;
            _sqlFolderPath = sqlFolderPath;
            _logger = logger;
        }

        public Task Migrate()
        {
            using var connection = _connectionFactory.Invoke();
            var evolve = new Evolve(connection, msg => _logger.LogInformation(msg))
            {
                Locations = [_sqlFolderPath],
                EnableClusterMode = true,
                StartVersion = new MigrationVersion(_startVersion),
                IsEraseDisabled = true,
                MetadataTableName = "migrations_changelog",
                OutOfOrder = true,
            };
            evolve.Migrate();
            return Task.CompletedTask;
        }
    }
}
