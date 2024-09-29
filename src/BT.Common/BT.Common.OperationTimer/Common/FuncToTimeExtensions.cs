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
            return (stopWatch.Elapsed, resultsArray.Length > 0 ? resultsArray : null);
        }
        internal static (TimeSpan TimeTaken, IReadOnlyCollection<object> Result) RunWithResult<TParam, TReturn>(this FuncToTime<TParam, TReturn> funcToTime)
        {
            var stopWatch = new Stopwatch();
            var resultsList = new List<object>();
            foreach (var item in funcToTime.Data)
            {
                if (funcToTime.IsReturnTypeTask)
                {
                    stopWatch.Start();
                    var result = funcToTime.Func.Invoke(item) as Task ?? throw new InvalidOperationException("Func must be a Func<TParam, Task> to return a Task result.");
                    result.GetAwaiter().GetResult();
                    stopWatch.Stop();
                }
                else
                {
                    stopWatch.Start();
                    var result = funcToTime.Func.Invoke(item);
                    stopWatch.Stop();
                    resultsList.Add(result);
                }
            }
            var resultsArray = resultsList.ToArray();
            return (stopWatch.Elapsed, resultsArray.Length > 0 ? resultsArray : null);
        }
        internal static TimeSpan Run<TParam, TReturn>(this FuncToTime<TParam, TReturn> funcToTime)
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
                return (stopWatch.Elapsed, resultsArray.Length > 0 ? resultsArray : null);
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
                return (stopWatch.Elapsed, resultsArray.Length > 0 ? resultsArray : null);
            }
        }
        internal static async Task<(TimeSpan TimeTaken, IReadOnlyCollection<object> Result)> RunWithResultAsync<TParam, TReturn>(this FuncToTime<TParam, TReturn> funcToTime, bool awaitAllAtOnce = false)
        {
            var stopWatch = new Stopwatch();
            if (funcToTime.IsReturnTypeTask)
            {
                if (awaitAllAtOnce)
                {
                    stopWatch.Start();
                    var jobLists = funcToTime.Data.Select(item => funcToTime.Func.Invoke(item) as Task ?? throw new InvalidOperationException("Func must be a Func<TParam, Task> to return a Task result."));
                    await Task.WhenAll(jobLists);
                    stopWatch.Stop();
                }
                else
                {
                    foreach (var item in funcToTime.Data)
                    {
                        stopWatch.Start();
                        var result = funcToTime.Func.Invoke(item);
                        stopWatch.Stop();
                    }
                }
                return (stopWatch.Elapsed, null);
            }
            else
            {
                return funcToTime.RunWithResult();
            }

        }
        internal static async Task<TimeSpan> RunAsync<TParam, TReturn>(this FuncToTime<TParam, TReturn> funcToTime, bool awaitAllAtOnce = false)
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