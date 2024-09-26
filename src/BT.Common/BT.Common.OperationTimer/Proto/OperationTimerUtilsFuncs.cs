using BT.Common.OperationTimer.Common;
using BT.Common.OperationTimer.Models;

namespace BT.Common.OperationTimer.Proto
{
    public static partial class OperationTimerUtils
    {
        /// <summary>
        /// This method will syncronously run the method and return a timespan for how long it took
        /// </summary>
        public static TimeSpan Time<TParam, TReturn>(Func<TParam?, TReturn?> func, TParam? data)
        {
            var funcToTime = new FuncToTime<TParam, TReturn>(func, data);
            return funcToTime.Run();
        }
        /// <summary>
        /// This method will syncronously run the method (against the all params provided) and return a timespan for how long it took
        /// </summary>
        public static TimeSpan Time<TParam, TReturn>(Func<TParam?, TReturn?> func, IEnumerable<TParam?> data)
        {
            var funcToTime = new FuncToTime<TParam, TReturn>(func, data);
            return funcToTime.Run();
        }
        /// <summary>
        /// This method will syncronously run the method and return a timespan for how long it took
        /// </summary>
        public static TimeSpan Time<TReturn>(Func<TReturn> func)
        {
            var funcToTime = new FuncToTime<object?, TReturn>(func.ToFuncWithParams(), [null]);
            return funcToTime.Run();
        }
        /// <summary>
        /// This method will asyncronously run the method (against the all params provided) and return a timespan for how long it took
        /// </summary>
        /// <param name="awaitAllAtOnce">
        /// If true, all tasks will be awaited at once. If false, tasks will be awaited as they complete.
        /// </param>
        public static async Task<TimeSpan> TimeAsync<TParam, TReturn>(Func<TParam?, TReturn?> func, IEnumerable<TParam?> data, bool awaitAllAtOnce = false)
        {
            var funcToTime = new FuncToTime<TParam, TReturn>(func, data);
            return await funcToTime.RunAsync(awaitAllAtOnce);
        }
        /// <summary>
        /// This method will asyncronously run the method and return a timespan for how long it took
        /// </summary>
        public static async Task<TimeSpan> TimeAsync<TParam, TReturn>(Func<TParam?, TReturn?> func, TParam? data)
        {
            var funcToTime = new FuncToTime<TParam, TReturn>(func, data);
            return await funcToTime.RunAsync(false);
        }
        /// <summary>
        /// This method will asyncronously run the method and return a timespan for how long it took
        /// </summary>
        public static async Task<TimeSpan> TimeAsync<TReturn>(Func<TReturn?> func)
        {
            var funcToTime = new FuncToTime<object?, TReturn>(func.ToFuncWithParams(), [null]);
            return await funcToTime.RunAsync(false);
        }
    }
}