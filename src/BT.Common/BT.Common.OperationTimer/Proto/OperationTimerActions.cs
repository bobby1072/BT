using BT.Common.OperationTimer.Models;

namespace BT.Common.OperationTimer.Proto
{
    public static partial class OperationTimerUtils
    {
        /// <summary>
        /// This method will syncronously run the method and return a timespan for how long it took
        /// </summary>
        public static TimeSpan Time<TParam>(Action<TParam> action, TParam data)
        {
            var actionToTime = new ActionToTime<TParam>(action, data);
            return actionToTime.Run();
        }
        /// <summary>
        /// This method will syncronously run the method (against the all params provided) and return a timespan for how long it took
        /// </summary>
        public static TimeSpan Time<TParam>(Action<TParam> action, IReadOnlyCollection<TParam> data)
        {
            var actionToTime = new ActionToTime<TParam>(action, data);
            return actionToTime.Run();
        }
        /// <summary>
        /// This method will syncronously run the method and return a timespan for how long it took
        /// </summary>
        public static TimeSpan Time(Action action)
        {
            var actionToTime = new ActionToTime(action);
            return actionToTime.Run();
        }
        /// <summary>
        /// This method will asyncronously run the method (against the all params provided) and return a timespan for how long it took
        /// </summary>
        /// <param name="awaitAllAtOnce">
        /// If true, all tasks will be awaited at once. If false, tasks will be awaited as they complete.
        /// </param>
        public static async Task<TimeSpan> TimeAsync<TParam>(Action<TParam> action, IReadOnlyCollection<TParam> data, bool awaitAllAtOnce = false)
        {
            var actionToTime = new ActionToTime<TParam>(action, data);
            return await actionToTime.RunAsync(awaitAllAtOnce);
        }
        /// <summary>
        /// This method will asyncronously run the method and return a timespan for how long it took
        /// </summary>
        public static async Task<TimeSpan> TimeAsync<TParam>(Action<TParam> action, TParam data)
        {
            var actionToTime = new ActionToTime<TParam>(action, data);
            return await actionToTime.RunAsync(false);
        }
        /// <summary>
        /// This method will asyncronously run the method and return a timespan for how long it took
        /// </summary>
        public static async Task<TimeSpan> TimeAsync(Action action)
        {
            var actionToTime = new ActionToTime(action);
            return await actionToTime.RunAsync(false);
        }
    }
}