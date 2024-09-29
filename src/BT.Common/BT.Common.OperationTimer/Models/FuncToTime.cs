
namespace BT.Common.OperationTimer.Models
{
    internal record FuncToTime<TParam, TReturn>
    {
        private static readonly Type _taskType = typeof(Task);
        public bool _isReturnTypeTask = typeof(TReturn) == _taskType;
        public bool _isReturnTypeTaskWithResult = typeof(TReturn).IsGenericType &&
                                                  typeof(TReturn).GetGenericTypeDefinition() == typeof(Task<>);
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