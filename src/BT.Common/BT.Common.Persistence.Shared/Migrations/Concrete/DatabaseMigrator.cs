using System.Data.Common;
using BT.Common.Persistence.Shared.Configurations;
using BT.Common.Persistence.Shared.Migrations.Abstract;
using EvolveDb;
using EvolveDb.Migration;
using Microsoft.Extensions.Logging;

namespace BT.Common.Persistence.Shared.Migrations.Concrete
{
    internal sealed class DatabaseMigrator : IMigrator
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
