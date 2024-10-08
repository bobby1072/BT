using BT.Common.OperationTimer.Common;
using BT.Common.OperationTimer.Models;

namespace BT.Common.OperationTimer.Proto
{
    public static partial class OperationTimerUtils
    {

        /// <summary>
        /// This method will syncronously run the method and return a timespan for how long it took
        /// </summary>
        public static TimeSpan Time<TParam, TReturn>(Func<TParam, Task<TReturn>> func, TParam data)
        {
            var funcToTime = new FuncToTime<TParam, Task<TReturn>>(func, data);
            return funcToTime.Execute().TimeTaken;
        }
        /// <summary>
        /// This method will syncronously run the method (against the all params provided) and return a timespan for how long it took
        /// </summary>
        public static TimeSpan Time<TParam, TReturn>(Func<TParam, Task<TReturn>> func, IEnumerable<TParam> data)
        {
            var funcToTime = new FuncToTime<TParam, Task<TReturn>>(func, data);
            return funcToTime.Execute().TimeTaken;
        }
        /// <summary>
        /// This method will syncronously run the method and return a timespan for how long it took
        /// </summary>
        public static TimeSpan Time<TReturn>(Func<Task<TReturn>> func)
        {
            var funcToTime = new FuncToTime<object, Task>(func.ToFuncWithParams(), [null]);
            return funcToTime.Execute().TimeTaken;
        }
        /// <summary>
        /// This method will syncronously run the method and return a timespan for how long it took
        /// </summary>
        public static TimeSpan Time<TParam>(Func<TParam, Task> func, TParam data)
        {
            var funcToTime = new FuncToTime<TParam, Task>(func, data);
            return funcToTime.Execute().TimeTaken;
        }
        /// <summary>
        /// This method will syncronously run the method (against the all params provided) and return a timespan for how long it took
        /// </summary>
        public static TimeSpan Time<TParam>(Func<TParam, Task> func, IEnumerable<TParam> data)
        {
            var funcToTime = new FuncToTime<TParam, Task>(func, data);
            return funcToTime.Execute().TimeTaken;
        }
        /// <summary>
        /// This method will syncronously run the method and return a timespan for how long it took
        /// </summary>
        public static TimeSpan Time(Func<Task> func)
        {
            var funcToTime = new FuncToTime<object, Task>(func.ToFuncWithParams(), [null]);
            return funcToTime.Execute().TimeTaken;
        }
        /// <summary>
        /// This method will syncronously run the method and return a timespan for how long it took
        /// </summary>
        public static TimeSpan Time<TParam, TReturn>(Func<TParam, TReturn> func, TParam data)
        {
            var funcToTime = new FuncToTime<TParam, TReturn>(func, data);
            return funcToTime.Execute().TimeTaken;
        }
        /// <summary>
        /// This method will syncronously run the method (against the all params provided) and return a timespan for how long it took
        /// </summary>
        public static TimeSpan Time<TParam, TReturn>(Func<TParam, TReturn> func, IEnumerable<TParam> data)
        {
            var funcToTime = new FuncToTime<TParam, TReturn>(func, data);
            return funcToTime.Execute().TimeTaken;
        }
        /// <summary>
        /// This method will syncronously run the method and return a timespan for how long it took
        /// </summary>
        public static TimeSpan Time<TReturn>(Func<TReturn> func)
        {
            var funcToTime = new FuncToTime<object, TReturn>(func.ToFuncWithParams(), [null]);
            return funcToTime.Execute().TimeTaken;
        }
        /// <summary>
        /// This method will asyncronously run the method (against the all params provided) and return a timespan for how long it took
        /// </summary>
        /// <param name="awaitAllAtOnce">
        /// If true, all tasks will be awaited at once. If false, tasks will be awaited as they complete.
        /// </param>
        public static async Task<TimeSpan> TimeAsync<TParam, TReturn>(Func<TParam, Task<TReturn>> func, IEnumerable<TParam> data, bool awaitAllAtOnce = false)
        {
            var funcToTime = new FuncToTime<TParam, Task<TReturn>>(func, data);
            return (await funcToTime.ExecuteAsync(awaitAllAtOnce)).TimeTaken;
        }
        /// <summary>
        /// This method will asyncronously run the method and return a timespan for how long it took
        /// </summary>
        public static async Task<TimeSpan> TimeAsync<TParam, TReturn>(Func<TParam, Task<TReturn>> func, TParam data)
        {
            var funcToTime = new FuncToTime<TParam, Task<TReturn>>(func, data);
            return (await funcToTime.ExecuteAsync(false)).TimeTaken;
        }
        /// <summary>
        /// This method will asyncronously run the method and return a timespan for how long it took
        /// </summary>
        public static async Task<TimeSpan> TimeAsync<TReturn>(Func<Task<TReturn>> func)
        {
            var funcToTime = new FuncToTime<object, Task<TReturn>>(func.ToFuncWithParams(), [null]);
            return (await funcToTime.ExecuteAsync(false)).TimeTaken;
        }
        /// <summary>
        /// This method will asyncronously run the method (against the all params provided) and return a timespan for how long it took
        /// </summary>
        /// <param name="awaitAllAtOnce">
        /// If true, all tasks will be awaited at once. If false, tasks will be awaited as they complete.
        /// </param>
        public static async Task<TimeSpan> TimeAsync<TParam>(Func<TParam, Task> func, IEnumerable<TParam> data, bool awaitAllAtOnce = false)
        {
            var funcToTime = new FuncToTime<TParam, Task>(func, data);
            return (await funcToTime.ExecuteAsync(awaitAllAtOnce)).TimeTaken;
        }
        /// <summary>
        /// This method will asyncronously run the method and return a timespan for how long it took
        /// </summary>
        public static async Task<TimeSpan> TimeAsync<TParam>(Func<TParam, Task> func, TParam data)
        {
            var funcToTime = new FuncToTime<TParam, Task>(func, data);
            return (await funcToTime.ExecuteAsync(false)).TimeTaken;
        }
        /// <summary>
        /// This method will asyncronously run the method and return a timespan for how long it took
        /// </summary>
        public static async Task<TimeSpan> TimeAsync(Func<Task> func)
        {
            var funcToTime = new FuncToTime<object, Task>(func.ToFuncWithParams(), [null]);
            return (await funcToTime.ExecuteAsync(false)).TimeTaken;
        }
        /// <summary>
        /// This method will asyncronously run the method (against the all params provided) and return a timespan for how long it took
        /// </summary>
        public static async Task<TimeSpan> TimeAsync<TParam>(Func<TParam, ValueTask> func, IEnumerable<TParam> data)
        {
            var funcToTime = new FuncToTime<TParam, ValueTask>(func, data);
            return (await funcToTime.ExecuteAsync()).TimeTaken;
        }
        /// <summary>
        /// This method will asyncronously run the method and return a timespan for how long it took
        /// </summary>
        public static async Task<TimeSpan> TimeAsync<TParam>(Func<TParam, ValueTask> func, TParam data)
        {
            var funcToTime = new FuncToTime<TParam, ValueTask>(func, data);
            return (await funcToTime.ExecuteAsync()).TimeTaken;
        }
        /// <summary>
        /// This method will asyncronously run the method and return a timespan for how long it took
        /// </summary>
        public static async Task<TimeSpan> TimeAsync(Func<ValueTask> func)
        {
            var funcToTime = new FuncToTime<object, ValueTask>(func.ToFuncWithParams(), [null]);
            return (await funcToTime.ExecuteAsync()).TimeTaken;
        }
        /// <summary>
        /// This method will asyncronously run the method (against the all params provided) and return a timespan for how long it took
        /// </summary>
        public static async Task<TimeSpan> TimeAsync<TParam, TReturn>(Func<TParam, ValueTask<TReturn>> func, IEnumerable<TParam> data)
        {
            var funcToTime = new FuncToTime<TParam, ValueTask<TReturn>>(func, data);
            return (await funcToTime.ExecuteAsync()).TimeTaken;
        }
        /// <summary>
        /// This method will asyncronously run the method and return a timespan for how long it took
        /// </summary>
        public static async Task<TimeSpan> TimeAsync<TParam, TReturn>(Func<TParam, ValueTask<TReturn>> func, TParam data)
        {
            var funcToTime = new FuncToTime<TParam, ValueTask<TReturn>>(func, data);
            return (await funcToTime.ExecuteAsync()).TimeTaken;
        }
        /// <summary>
        /// This method will asyncronously run the method and return a timespan for how long it took
        /// </summary>
        public static async Task<TimeSpan> TimeAsync<TReturn>(Func<ValueTask<TReturn>> func)
        {
            var funcToTime = new FuncToTime<object, ValueTask<TReturn>>(func.ToFuncWithParams(), [null]);
            return (await funcToTime.ExecuteAsync()).TimeTaken;
        }
    }
}