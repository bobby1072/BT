namespace BT.Common.Helpers.TypeFor
{
    internal class ActualTypeFor<T> : TypeFor<T>
    {
        private T _value;
        public override T Value { get => _value; }
        public ActualTypeFor(T value) 
        { 
            _value = value;
        }
    }
}
