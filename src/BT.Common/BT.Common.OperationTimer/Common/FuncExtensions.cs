namespace BT.Common.OperationTimer.Common
{
    internal static class FuncExtensions
    {
        public static Func<object?, TReturn> ToFuncWithParams<TReturn>(this Func<TReturn> func)
        {
            return _ =>
            {
                return func.Invoke();
            };
        }
    }
}