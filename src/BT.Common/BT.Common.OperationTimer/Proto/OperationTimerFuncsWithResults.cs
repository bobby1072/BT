using BT.Common.OperationTimer.Models;

namespace BT.Common.OperationTimer.Proto
{
    public static partial class OperationTimerUtils
    {
        /// <summary>
        /// This method will syncronously run the method and return a timespan for how long it took
        /// </summary>
        public static (TimeSpan, TReturn) TimeWithResults<TParam, TReturn>(Func<TParam, TReturn> func, TParam data)
        {
            var funcToTime = new FuncToTime<TParam, TReturn>(func, data);
            var timedResult = funcToTime.RunWithResult();
            return (timedResult.TimeTaken, (TReturn?)timedResult.Result?.First());
        }
        /// <summary>
        /// This method will syncronously run the method (against the all params provided) and return a timespan for how long it took
        /// </summary>
        public static (TimeSpan, IReadOnlyCollection<TReturn>) TimeWithResults<TParam, TReturn>(Func<TParam, TReturn> func, IEnumerable<TParam> data)
        {
            var funcToTime = new FuncToTime<TParam, TReturn>(func, data);
            var timedResult = funcToTime.RunWithResult();
            return (timedResult.TimeTaken, (IReadOnlyCollection<TReturn>?)timedResult.Result);
        }
        /// <summary>
        /// This method will syncronously run the method and return a timespan for how long it took
        /// </summary>
        public static (TimeSpan, TReturn) TimeWithResults<TReturn>(Func<TReturn> func)
        {
            var funcToTime = new FuncToTime<object?, TReturn>(x => func.Invoke(), [null]);
            var timedResult = funcToTime.RunWithResult();
            return (timedResult.TimeTaken, (TReturn?)timedResult.Result?.First());
        }
        /// <summary>
        /// This method will asyncronously run the method (against the all params provided) and return a timespan for how long it took
        /// </summary>
        /// <param name="awaitAllAtOnce">
        /// If true, all tasks will be awaited at once. If false, tasks will be awaited as they complete.
        /// </param>
        public static async Task<(TimeSpan, IReadOnlyCollection<TReturn>)> TimeAsyncWithResults<TParam, TReturn>(Func<TParam, TReturn> func, IEnumerable<TParam> data, bool awaitAllAtOnce = false)
        {
            var funcToTime = new FuncToTime<TParam, TReturn>(func, data);
            var timedResult = await funcToTime.RunWithResultAsync(awaitAllAtOnce);
            return (timedResult.TimeTaken, (IReadOnlyCollection<TReturn>?)timedResult.Result);
        }
        /// <summary>
        /// This method will asyncronously run the method and return a timespan for how long it took
        /// </summary>
        public static async Task<(TimeSpan, TReturn)> TimeAsyncWithResults<TParam, TReturn>(Func<TParam, TReturn> func, TParam data)
        {
            var funcToTime = new FuncToTime<TParam, TReturn>(func, data);
            var timedResult = await funcToTime.RunWithResultAsync(false);
            return (timedResult.TimeTaken, (TReturn?)timedResult.Result?.First());
        }
        /// <summary>
        /// This method will asyncronously run the method and return a timespan for how long it took
        /// </summary>
        public static async Task<(TimeSpan, TReturn)> TimeAsyncWithResults<TReturn>(Func<TReturn> func)
        {
            var funcToTime = new FuncToTime<object?, TReturn>(x => func.Invoke(), [null]);
            var timedResult = await funcToTime.RunWithResultAsync(false);
            return (timedResult.TimeTaken, (TReturn?)timedResult.Result?.First());
        }
    }
}