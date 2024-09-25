namespace BT.Common.OperationTimer.Proto
{
    public static class FuncExtensions
    {
        /// <summary>
        /// This method will syncronously run the method and return a timespan for how long it took
        /// </summary>
        public static TimeSpan Time<TParam, TReturn>(this Func<TParam, TReturn> func, TParam data) => OperationTimerUtils.Time(func, data);
        /// <summary>
        /// This method will syncronously run the method (against the all params provided) and return a timespan for how long it took
        /// </summary>
        public static TimeSpan Time<TParam, TReturn>(this Func<TParam, TReturn> func, IEnumerable<TParam> data) => OperationTimerUtils.Time(func, data);
        /// <summary>
        /// This method will syncronously run the method and return a timespan for how long it took
        /// </summary>
        public static TimeSpan Time<TReturn>(this Func<TReturn> func) => OperationTimerUtils.Time(func);
        /// <summary>
        /// This method will asyncronously run the method (against the all params provided) and return a timespan for how long it took
        /// </summary>
        /// <param name="awaitAllAtOnce">
        /// If true, all tasks will be awaited at once. If false, tasks will be awaited as they complete.
        /// </param>
        public static async Task<TimeSpan> TimeAsync<TParam, TReturn>(this Func<TParam, TReturn> func, IEnumerable<TParam> data, bool awaitAllAtOnce = false) => await OperationTimerUtils.TimeAsync(func, data, awaitAllAtOnce);
        /// <summary>
        /// This method will asyncronously run the method and return a timespan for how long it took
        /// </summary>
        public static async Task<TimeSpan> TimeAsync<TParam, TReturn>(this Func<TParam, TReturn> func, TParam data) => await OperationTimerUtils.TimeAsync(func, data);
        /// <summary>
        /// This method will asyncronously run the method and return a timespan for how long it took
        /// </summary>
        public static async Task<TimeSpan> TimeAsync<TReturn>(this Func<TReturn> func) => await OperationTimerUtils.TimeAsync(func);
    }
}