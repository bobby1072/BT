
namespace BT.Common.SpeedTest.Models.Input
{
    internal class FuncToTime<TParam, TReturn>(Func<TParam, TReturn> func, IReadOnlyCollection<TParam> data)
    {
        public Func<TParam, TReturn> Func { get; init; } = func;
        public IReadOnlyCollection<TParam> Data { get; init; } = data;
        public FuncToTime(Func<TParam, TReturn> func, TParam data) : this(func, [data]) { }
    }
    internal class FuncToTime<TReturn>(Func<TReturn> func)
    {
        public Func<TReturn> Func { get; init; } = func;
    }
}
