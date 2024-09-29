
using BT.Common.OperationTimer.Common;

namespace BT.Common.OperationTimer.Models
{
    internal record FuncToTime<TParam, TReturn>
    {
        public static Type TReturnType = typeof(TReturn);
        public bool IsReturnTypeTask = TReturnType == FuncToTimeExtensions.TaskType;
        public bool IsReturnTypeTaskWithResult = TReturnType.IsGenericType &&
                                                  TReturnType.GetGenericTypeDefinition() == typeof(Task<>);
        public Func<TParam, TReturn> Func { get; init; }
        public IReadOnlyCollection<TParam> Data { get; init; }
        internal FuncToTime(Func<TParam, TReturn> func, TParam data)
        {
            Func = func;
            Data = [data];
        }
        internal FuncToTime(Func<TParam, TReturn> func, IEnumerable<TParam> data)
        {
            Func = func;
            Data = data.ToArray();
        }

    }
}