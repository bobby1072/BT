namespace BT.Common.Helpers.TypeFor
{
    public sealed class TypeFor<T>
    {
        public Type ActualType { get; } = typeof(T);

        public static TypeFor<T> GetTypeFor()
        {
            return new TypeFor<T>();
        }
    }
}
