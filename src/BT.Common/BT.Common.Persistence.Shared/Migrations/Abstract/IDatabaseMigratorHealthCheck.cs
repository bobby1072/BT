using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BT.Common.Persistence.Shared.Migrations.Abstract;

/// <summary>
/// Defines a health check interface for monitoring the status of database migrations. This interface extends the
/// </summary>
/// <remarks>
/// The <see cref="IDatabaseMigratorHealthCheck"/> interface provides a method to set
/// the migration completion status, which can be used to indicate whether the database migrations have been successfully completed or not. This allows for better monitoring and alerting in case of migration failures.
/// </remarks>
public interface IDatabaseMigratorHealthCheck : IHealthCheck
{
    internal void SetMigrationCompleted(bool isMigrationCompleted);
}
