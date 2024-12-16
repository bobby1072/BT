namespace BT.Common.Persistence.Shared.Models
{
    public record DbSaveResult<TModel> : DbResult<IReadOnlyCollection<TModel>> where TModel : class
    {
        public TModel FirstResult { get => Data.First(); } 
        public TModel? FirstResultOrDefault { get => Data.FirstOrDefault(); } 
        public DbSaveResult(IReadOnlyCollection<TModel>? models = null) : base(models?.Count > 0, models ?? Array.Empty<TModel>()) { }
    }
}
