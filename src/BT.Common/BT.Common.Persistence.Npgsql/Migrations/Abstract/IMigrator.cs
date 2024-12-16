namespace BT.Common.Persistence.Npgsql.Migrations.Abstract
{
    internal interface IMigrator
    {
        public Task Migrate();
    }
}