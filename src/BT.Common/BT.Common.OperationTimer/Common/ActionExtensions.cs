
namespace BT.Common.OperationTimer.Common
{
    internal static class ActionExtensions
    {
        internal static Func<TParam, object> ToFuncWithParams<TParam>(this Action<TParam> action)
        {
            return x =>
            {
                action.Invoke(x);
                return null;
            };
        }
        internal static Func<object, object> ToFuncWithParams(this Action action)
        {
            return x =>
            {
                action.Invoke();
                return null;
            };
        }
    }
}