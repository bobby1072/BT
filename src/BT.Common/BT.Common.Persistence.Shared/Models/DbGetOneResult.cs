namespace BT.Common.Persistence.Shared.Models
{
    public record DbGetOneResult<TModel> : DbResult<TModel>
    {
        public DbGetOneResult(TModel model) : base(model is not null, model) { }
    }
}
