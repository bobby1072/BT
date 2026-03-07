using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BT.Common.Persistence.Shared.Migrations.Abstract;

public interface IDatabaseMigratorHealthCheck : IHealthCheck
{
    internal void SetMigrationCompleted(bool isMigrationCompleted);
}
