namespace BT.Common.Helpers.TypeFor
{
    public abstract class TypeFor<T> 
    {
        public abstract T Value { get; }
        public readonly Type ActualType = typeof(T);
    }
}
