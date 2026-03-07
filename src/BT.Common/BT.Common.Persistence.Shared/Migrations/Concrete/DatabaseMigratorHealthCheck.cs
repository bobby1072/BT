using BT.Common.Persistence.Shared.Migrations.Abstract;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BT.Common.Persistence.Shared.Migrations.Concrete;

internal sealed class DatabaseMigratorHealthCheck : IDatabaseMigratorHealthCheck
{
    private bool _migrationCompleted = false;

    public void SetMigrationCompleted(bool isMigrationCompleted)
    {
        _migrationCompleted = isMigrationCompleted;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default
    )
    {
        return Task.FromResult(
            _migrationCompleted
                ? HealthCheckResult.Healthy("The database migrator is finished.")
                : HealthCheckResult.Unhealthy("The database migrator is still running.")
        );
    }
}
