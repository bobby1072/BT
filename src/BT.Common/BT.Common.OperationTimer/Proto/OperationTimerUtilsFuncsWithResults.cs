using BT.Common.OperationTimer.Common;
using BT.Common.OperationTimer.Models;

namespace BT.Common.OperationTimer.Proto
{
    public static partial class OperationTimerUtils
    {
        /// <summary>
        /// This method will syncronously run the method and return a timespan for how long it took
        /// </summary>
        /// <returns>The time taken to run the function and results if there are any</returns>
        public static (TimeSpan, Task) TimeWithResults<TParam>(Func<TParam, Task> func, TParam data)
        {
            var funcToTime = new FuncToTime<TParam, Task>(func, data);
            var timedResult = funcToTime.Execute();
            return (timedResult.TimeTaken, timedResult.Result.First());
        }
        /// <summary>
        /// This method will syncronously run the method (against the all params provided) and return a timespan for how long it took
        /// </summary>
        /// <returns>The time taken to run the function and results if there are any</returns>
        public static (TimeSpan, IReadOnlyCollection<Task>) TimeWithResults<TParam>(Func<TParam, Task> func, IEnumerable<TParam> data)
        {
            var funcToTime = new FuncToTime<TParam, Task>(func, data);
            var timedResult = funcToTime.Execute();
            return (timedResult.TimeTaken, timedResult.Result);
        }
        /// <summary>
        /// This method will syncronously run the method and return a timespan for how long it took
        /// </summary>
        /// <returns>The time taken to run the function and results if there are any</returns>
        public static (TimeSpan, Task) TimeWithResults(Func<Task> func)
        {
            var funcToTime = new FuncToTime<object, Task>(func.ToFuncWithParams(), [null]);
            var timedResult = funcToTime.Execute();
            return (timedResult.TimeTaken, timedResult.Result.First());
        }
        /// <summary>
        /// This method will syncronously run the method and return a timespan for how long it took
        /// </summary>
        /// <returns>The time taken to run the function and results if there are any</returns>
        public static (TimeSpan, TReturn) TimeWithResults<TParam, TReturn>(Func<TParam, TReturn> func, TParam data)
        {
            var funcToTime = new FuncToTime<TParam, TReturn>(func, data);
            var timedResult = funcToTime.Execute();
            return (timedResult.TimeTaken, timedResult.Result.First());
        }
        /// <summary>
        /// This method will syncronously run the method (against the all params provided) and return a timespan for how long it took
        /// </summary>
        /// <returns>The time taken to run the function and results if there are any</returns>
        public static (TimeSpan, IReadOnlyCollection<TReturn>) TimeWithResults<TParam, TReturn>(Func<TParam, TReturn> func, IEnumerable<TParam> data)
        {
            var funcToTime = new FuncToTime<TParam, TReturn>(func, data);
            var timedResult = funcToTime.Execute();
            return (timedResult.TimeTaken, timedResult.Result);
        }
        /// <summary>
        /// This method will syncronously run the method and return a timespan for how long it took
        /// </summary>
        /// <returns>The time taken to run the function and results if there are any</returns>
        public static (TimeSpan, TReturn) TimeWithResults<TReturn>(Func<TReturn> func)
        {
            var funcToTime = new FuncToTime<object, TReturn>(func.ToFuncWithParams(), [null]);
            var timedResult = funcToTime.Execute();
            return (timedResult.TimeTaken, timedResult.Result.First());
        }
        /// <summary>
        /// This method will syncronously run the method and return a timespan for how long it took
        /// </summary>
        /// <returns>The time taken to run the function and results if there are any</returns>
        public static (TimeSpan, TReturn) TimeWithResults<TParam, TReturn>(Func<TParam, Task<TReturn>> func, TParam data)
        {
            var funcToTime = new FuncToTime<TParam, Task<TReturn>>(func, data);
            var timedResult = funcToTime.Execute();
            return (timedResult.TimeTaken, timedResult.Result.First());
        }
        /// <summary>
        /// This method will syncronously run the method (against the all params provided) and return a timespan for how long it took
        /// </summary>
        /// <returns>The time taken to run the function and results if there are any</returns>
        public static (TimeSpan, IReadOnlyCollection<TReturn>) TimeWithResults<TParam, TReturn>(Func<TParam, Task<TReturn>> func, IEnumerable<TParam> data)
        {
            var funcToTime = new FuncToTime<TParam, Task<TReturn>>(func, data);
            var timedResult = funcToTime.Execute();
            return (timedResult.TimeTaken, timedResult.Result);
        }
        /// <summary>
        /// This method will syncronously run the method and return a timespan for how long it took
        /// </summary>
        /// <returns>The time taken to run the function and results if there are any</returns>
        public static (TimeSpan, TReturn) TimeWithResults<TReturn>(Func<Task<TReturn>> func)
        {
            var funcToTime = new FuncToTime<object, Task<TReturn>>(func.ToFuncWithParams(), [null]);
            var timedResult = funcToTime.Execute();
            return (timedResult.TimeTaken, timedResult.Result.First());
        }
        /// <summary>
        /// This method will asyncronously run the method (against the all params provided) and return a timespan for how long it took
        /// </summary>
        /// <param name="awaitAllAtOnce">
        /// If true, all tasks will be awaited at once. If false, tasks will be awaited as they complete.
        /// </param>
        /// <returns>The time taken to run the function and results if there are any</returns>
        public static async Task<(TimeSpan, IReadOnlyCollection<TReturn>)> TimeWithResultsAsync<TParam, TReturn>(Func<TParam, Task<TReturn>> func, IEnumerable<TParam> data, bool awaitAllAtOnce = false)
        {
            var funcToTime = new FuncToTime<TParam, Task<TReturn>>(func, data);
            var timedResult = await funcToTime.ExecuteAsync(awaitAllAtOnce);
            return (timedResult.TimeTaken, timedResult.Result);
        }
        /// <summary>
        /// This method will asyncronously run the method and return a timespan for how long it took
        /// </summary>
        /// <returns>The time taken to run the function and results if there are any</returns>
        public static async Task<(TimeSpan, TReturn)> TimeWithResultsAsync<TParam, TReturn>(Func<TParam, Task<TReturn>> func, TParam data)
        {
            var funcToTime = new FuncToTime<TParam, Task<TReturn>>(func, data);
            var timedResult = await funcToTime.ExecuteAsync(false);
            return (timedResult.TimeTaken, timedResult.Result.First());
        }
        /// <summary>
        /// This method will asyncronously run the method and return a timespan for how long it took
        /// </summary>
        /// <returns>The time taken to run the function and results if there are any</returns>
        public static async Task<(TimeSpan, TReturn)> TimeWithResultsAsync<TReturn>(Func<Task<TReturn>> func)
        {
            var funcToTime = new FuncToTime<object, Task<TReturn>>(func.ToFuncWithParams(), [null]);
            var timedResult = await funcToTime.ExecuteAsync(false);
            return (timedResult.TimeTaken, timedResult.Result.First());
        }
        /// <summary>
        /// This method will asyncronously run the method (against the all params provided) and return a timespan for how long it took
        /// </summary>
        /// <param name="awaitAllAtOnce">
        /// If true, all tasks will be awaited at once. If false, tasks will be awaited as they complete.
        /// </param>
        /// <returns>The time taken to run the function and results if there are any</returns>
        public static async Task<(TimeSpan, IReadOnlyCollection<Task>)> TimeWithResultsAsync<TParam>(Func<TParam, Task> func, IEnumerable<TParam> data, bool awaitAllAtOnce = false)
        {
            var funcToTime = new FuncToTime<TParam, Task>(func, data);
            var timedResult = await funcToTime.ExecuteAsync(awaitAllAtOnce);
            return (timedResult.TimeTaken, timedResult.Result);
        }
        /// <summary>
        /// This method will asyncronously run the method and return a timespan for how long it took
        /// </summary>
        /// <returns>The time taken to run the function and results if there are any</returns>
        public static async Task<(TimeSpan, Task)> TimeWithResultsAsync<TParam>(Func<TParam, Task> func, TParam data)
        {
            var funcToTime = new FuncToTime<TParam, Task>(func, data);
            var timedResult = await funcToTime.ExecuteAsync(false);
            return (timedResult.TimeTaken, timedResult.Result.First());
        }
        /// <summary>
        /// This method will asyncronously run the method and return a timespan for how long it took
        /// </summary>
        /// <returns>The time taken to run the function and results if there are any</returns>
        public static async Task<(TimeSpan, Task)> TimeWithResultsAsync(Func<Task> func)
        {
            var funcToTime = new FuncToTime<object, Task>(func.ToFuncWithParams(), [null]);
            var timedResult = await funcToTime.ExecuteAsync(false);
            return (timedResult.TimeTaken, timedResult.Result.First());
        }
        /// <summary>
        /// This method will asyncronously run the method (against the all params provided) and return a timespan for how long it took
        /// </summary>
        /// <returns>The time taken to run the function and results if there are any</returns>
        public static async Task<(TimeSpan, IReadOnlyCollection<ValueTask>)> TimeWithResultsAsync<TParam>(Func<TParam, ValueTask> func, IEnumerable<TParam> data, bool awaitAllAtOnce = false)
        {
            var funcToTime = new FuncToTime<TParam, ValueTask>(func, data);
            var timedResult = await funcToTime.ExecuteAsync();
            return (timedResult.TimeTaken, timedResult.Result);
        }
        /// <summary>
        /// This method will asyncronously run the method and return a timespan for how long it took
        /// </summary>
        /// <returns>The time taken to run the function and results if there are any</returns>
        public static async Task<(TimeSpan, ValueTask)> TimeWithResultsAsync<TParam>(Func<TParam, ValueTask> func, TParam data)
        {
            var funcToTime = new FuncToTime<TParam, ValueTask>(func, data);
            var timedResult = await funcToTime.ExecuteAsync();
            return (timedResult.TimeTaken, timedResult.Result.First());
        }
        /// <summary>
        /// This method will asyncronously run the method and return a timespan for how long it took
        /// </summary>
        /// <returns>The time taken to run the function and results if there are any</returns>
        public static async Task<(TimeSpan, ValueTask)> TimeWithResultsAsync(Func<ValueTask> func)
        {
            var funcToTime = new FuncToTime<object, ValueTask>(func.ToFuncWithParams(), [null]);
            var timedResult = await funcToTime.ExecuteAsync();
            return (timedResult.TimeTaken, timedResult.Result.First());
        }
        /// <summary>
        /// This method will asyncronously run the method (against the all params provided) and return a timespan for how long it took
        /// </summary>
        /// <returns>The time taken to run the function and results if there are any</returns>
        public static async Task<(TimeSpan, IReadOnlyCollection<TReturn>)> TimeWithResultsAsync<TParam, TReturn>(Func<TParam, ValueTask<TReturn>> func, IEnumerable<TParam> data)
        {
            var funcToTime = new FuncToTime<TParam, ValueTask<TReturn>>(func, data);
            var timedResult = await funcToTime.ExecuteAsync();
            return (timedResult.TimeTaken, timedResult.Result);
        }
        /// <summary>
        /// This method will asyncronously run the method and return a timespan for how long it took
        /// </summary>
        /// <returns>The time taken to run the function and results if there are any</returns>
        public static async Task<(TimeSpan, TReturn)> TimeWithResultsAsync<TParam, TReturn>(Func<TParam, ValueTask<TReturn>> func, TParam data)
        {
            var funcToTime = new FuncToTime<TParam, ValueTask<TReturn>>(func, data);
            var timedResult = await funcToTime.ExecuteAsync();
            return (timedResult.TimeTaken, timedResult.Result.First());
        }
        /// <summary>
        /// This method will asyncronously run the method and return a timespan for how long it took
        /// </summary>
        /// <returns>The time taken to run the function and results if there are any</returns>
        public static async Task<(TimeSpan, TReturn)> TimeWithResultsAsync<TReturn>(Func<ValueTask<TReturn>> func)
        {
            var funcToTime = new FuncToTime<object, ValueTask<TReturn>>(func.ToFuncWithParams(), [null]);
            var timedResult = await funcToTime.ExecuteAsync();
            return (timedResult.TimeTaken, timedResult.Result.First());
        }
    }
}