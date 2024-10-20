namespace BT.Common.Helpers.TypeFor
{
    public static class TypeForUtils
    {
        public static TypeFor<T> GetTypeFor<T>()
        {
            return new TypeFor<T>();
        }
    }
}
