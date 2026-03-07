namespace BT.Common.Persistence.Shared.Migrations.Abstract
{
    public interface IMigrator
    {
        internal Task Migrate();
    }
}
