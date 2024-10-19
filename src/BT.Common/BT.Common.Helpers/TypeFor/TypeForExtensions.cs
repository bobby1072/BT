namespace BT.Common.Helpers.TypeFor
{
    public static class TypeForExtensions
    {
        public static TypeFor<T> GetTypeFor<T>(this T value)
        {
            return new TypeFor<T>();
        }
    }
}
