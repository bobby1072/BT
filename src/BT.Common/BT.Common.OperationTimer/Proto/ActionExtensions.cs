using BT.Common.OperationTimer.Models;

namespace BT.Common.OperationTimer.Proto
{
    public static class ActionExtensions
    {
        /// <summary>
        /// This method will syncronously run the method and return a timespan for how long it took
        /// </summary>
        public static TimeSpan Time<TParam>(this Action<TParam> action, TParam data) => OperationTimer.Time(action, data);
        /// <summary>
        /// This method will syncronously run the method (against the all params provided) and return a timespan for how long it took
        /// </summary>
        public static TimeSpan Time<TParam>(this Action<TParam> action, IReadOnlyCollection<TParam> data) => OperationTimer.Time(action, data);
        /// <summary>
        /// This method will syncronously run the method and return a timespan for how long it took
        /// </summary>
        public static TimeSpan Time(this Action action) => OperationTimer.Time(action);
        /// <summary>
        /// This method will asyncronously run the method (against the all params provided) and return a timespan for how long it took
        /// </summary>
        /// <param name="awaitAllAtOnce">
        /// If true, all tasks will be awaited at once. If false, tasks will be awaited as they complete.
        /// </param>
        public static async Task<TimeSpan> TimeAsync<TParam>(this Action<TParam> action, IReadOnlyCollection<TParam> data, bool awaitAllAtOnce = false) => await OperationTimer.TimeAsync(action, data, awaitAllAtOnce);
        /// <summary>
        /// This method will asyncronously run the method and return a timespan for how long it took
        /// </summary>
        public static async Task<TimeSpan> TimeAsync<TParam>(this Action<TParam> action, TParam data) => await OperationTimer.TimeAsync(action, data);
        /// <summary>
        /// This method will asyncronously run the method and return a timespan for how long it took
        /// </summary>
        public static async Task<TimeSpan> TimeAsync(this Action action) => await OperationTimer.TimeAsync(action);
    }
}