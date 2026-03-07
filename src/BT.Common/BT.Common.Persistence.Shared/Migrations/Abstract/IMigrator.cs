namespace BT.Common.Persistence.Shared.Migrations.Abstract
{
    internal interface IMigrator
    {
        Task Migrate();
    }
}
