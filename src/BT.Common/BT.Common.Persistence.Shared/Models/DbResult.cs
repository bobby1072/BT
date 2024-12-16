namespace BT.Common.Persistence.Shared.Models
{
    public record DbResult
    {
        public bool IsSuccessful { get; init; }
        public DbResult(bool isSucccessful)
        {
            IsSuccessful = isSucccessful;
        }
    }
    public record DbResult<T> : DbResult
    {
        public T Data { get; init; }
        public DbResult(bool isSuccess, T data) : base(isSuccess)
        {
            Data = data;
        }
    }
}
