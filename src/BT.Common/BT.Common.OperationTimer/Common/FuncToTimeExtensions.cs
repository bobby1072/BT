using System.Diagnostics;
using BT.Common.OperationTimer.Models;

namespace BT.Common.OperationTimer.Common
{
    internal static class FuncToTimeExtensions
    {
        public static readonly Type TaskType = typeof(Task);
        internal static (TimeSpan TimeTaken, IReadOnlyCollection<object> Result) RunWithResult<TParam, TReturn>(this FuncToTime<TParam, Task<TReturn>> funcToTime)
        {
            var stopWatch = new Stopwatch();
            var resultsList = new List<object>();
            foreach (var item in funcToTime.Data)
            {
                stopWatch.Start();
                var funcWithReturn = (funcToTime.Func.Invoke(item) as Task) ?? throw new InvalidOperationException("Func must be a Func<TParam, Task<TReturn>> to return a Task result.");
                funcWithReturn.GetAwaiter().GetResult();
                stopWatch.Stop();
                resultsList.Add(CastTaskToObjectAsync(funcWithReturn).GetAwaiter().GetResult());
            }
            var resultsArray = resultsList.ToArray();
            return (stopWatch.Elapsed, resultsArray);
        }
        internal static (TimeSpan TimeTaken, IReadOnlyCollection<Task> Result) RunWithResult<TParam>(this FuncToTime<TParam, Task> funcToTime)
        {
            var stopWatch = new Stopwatch();
            foreach (var item in funcToTime.Data)
            {
                stopWatch.Start();
                var result = funcToTime.Func.Invoke(item);
                result.GetAwaiter().GetResult();
                stopWatch.Stop();
            }
            return (stopWatch.Elapsed, funcToTime.Data.Select(item => Task.CompletedTask).ToArray());
        }
        internal static (TimeSpan TimeTaken, IReadOnlyCollection<object> Result) RunWithResult<TParam, TReturn>(this FuncToTime<TParam, TReturn> funcToTime)
        {
            var stopWatch = new Stopwatch();
            var resultsList = new List<object>();
            foreach (var item in funcToTime.Data)
            {
                stopWatch.Start();
                var result = funcToTime.Func.Invoke(item);
                stopWatch.Stop();
                resultsList.Add(result);
            }
            var resultsArray = resultsList.ToArray();
            return (stopWatch.Elapsed, resultsArray);
        }
        internal static TimeSpan Run<TParam, TReturn>(this FuncToTime<TParam, TReturn> funcToTime)
        {

            return funcToTime.RunWithResult().TimeTaken;
        }
        internal static TimeSpan Run<TParam, TReturn>(this FuncToTime<TParam, Task<TReturn>> funcToTime)
        {

            return funcToTime.RunWithResult().TimeTaken;
        }
        internal static TimeSpan Run<TParam>(this FuncToTime<TParam, Task> funcToTime)
        {

            return funcToTime.RunWithResult().TimeTaken;
        }
        internal static async Task<(TimeSpan TimeTaken, IReadOnlyCollection<object> Result)> RunWithResultAsync<TParam, TReturn>(this FuncToTime<TParam, Task<TReturn>> funcToTime, bool awaitAllAtOnce = false)
        {
            var stopWatch = new Stopwatch();

            if (awaitAllAtOnce)
            {
                stopWatch.Start();
                var jobLists = funcToTime.Data.Select(item => (funcToTime.Func.Invoke(item) as Task) ?? throw new InvalidOperationException("Func must be a Func<TParam, Task<TReturn>> to return a Task result."));
                await Task.WhenAll(jobLists);
                stopWatch.Stop();
                var resultsArray = jobLists.Select(CastTaskToObjectAsync).ToArray() as object[];
                return (stopWatch.Elapsed, resultsArray);
            }
            else
            {
                var results = new List<object>();
                foreach (var item in funcToTime.Data)
                {
                    stopWatch.Start();
                    var task = (funcToTime.Func.Invoke(item) as Task) ?? throw new InvalidOperationException("Func must be a Func<TParam, Task<TReturn>> to return a Task result.");
                    await task;
                    stopWatch.Stop();
                    results.Add(await CastTaskToObjectAsync(task));
                }
                var resultsArray = results.ToArray();
                return (stopWatch.Elapsed, resultsArray);
            }
        }
        internal static async Task<(TimeSpan TimeTaken, IReadOnlyCollection<Task> Result)> RunWithResultAsync<TParam>(this FuncToTime<TParam, Task> funcToTime, bool awaitAllAtOnce = false)
        {
            var stopWatch = new Stopwatch();
            if (awaitAllAtOnce)
            {
                stopWatch.Start();
                var jobLists = funcToTime.Data.Select(funcToTime.Func.Invoke);
                await Task.WhenAll(jobLists);
                stopWatch.Stop();
            }
            else
            {
                foreach (var item in funcToTime.Data)
                {
                    stopWatch.Start();
                    await funcToTime.Func.Invoke(item);
                    stopWatch.Stop();
                }
            }
            return (stopWatch.Elapsed, funcToTime.Data.Select(item => Task.CompletedTask).ToArray());
        }
        internal static async Task<TimeSpan> RunAsync<TParam, TReturn>(this FuncToTime<TParam, Task<TReturn>> funcToTime, bool awaitAllAtOnce = false)
        {
            return (await funcToTime.RunWithResultAsync(awaitAllAtOnce)).TimeTaken;
        }
        internal static async Task<TimeSpan> RunAsync<TParam>(this FuncToTime<TParam, Task> funcToTime, bool awaitAllAtOnce = false)
        {
            return (await funcToTime.RunWithResultAsync(awaitAllAtOnce)).TimeTaken;
        }
        private static async Task<object> CastTaskToObjectAsync(Task task)
        {
            var resultProperty = task.GetType().GetProperty("Result");

            await task;
            return resultProperty?.GetValue(task);
        }
    }
}