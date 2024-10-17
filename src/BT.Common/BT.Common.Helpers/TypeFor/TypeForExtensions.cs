namespace BT.Common.Helpers.TypeFor
{
    public static class TypeForExtensions
    {
        public static TypeFor<T> GetType<T>(this T baseObject)
        {
            return new ActualTypeFor<T>(baseObject);
        }
    }
}
