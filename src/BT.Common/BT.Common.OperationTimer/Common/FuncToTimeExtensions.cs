using System.Diagnostics;
using BT.Common.OperationTimer.Models;

namespace BT.Common.OperationTimer.Common
{
    internal static class FuncToTimeExtensions
    {
        internal static (TimeSpan TimeTaken, IReadOnlyCollection<TReturn> Result) RunWithResult<TParam, TReturn>(this FuncToTime<TParam, Task<TReturn>> funcToTime)
        {
            var stopWatch = new Stopwatch();
            var resultsList = new List<TReturn>();
            foreach (var item in funcToTime.Data)
            {
                stopWatch.Start();
                var result = funcToTime.Func.Invoke(item).GetAwaiter().GetResult();
                stopWatch.Stop();
                resultsList.Add(result);
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
        internal static (TimeSpan TimeTaken, IReadOnlyCollection<TReturn> Result) RunWithResult<TParam, TReturn>(this FuncToTime<TParam, TReturn> funcToTime)
        {
            var stopWatch = new Stopwatch();
            var resultsList = new List<TReturn>();
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
        internal static async Task<(TimeSpan TimeTaken, IReadOnlyCollection<TReturn> Result)> RunWithResultAsync<TParam, TReturn>(this FuncToTime<TParam, Task<TReturn>> funcToTime, bool awaitAllAtOnce = false)
        {
            var stopWatch = new Stopwatch();

            if (awaitAllAtOnce)
            {
                var jobList = new List<Task<TReturn>>();
                stopWatch.Start();
                foreach (var item in funcToTime.Data)
                {
                    jobList.Add(funcToTime.Func.Invoke(item));
                }
                await Task.WhenAll(jobList);
                stopWatch.Stop();
                var resultsArray = jobList.Select(x => x.Result).ToArray();
                return (stopWatch.Elapsed, resultsArray);
            }
            else
            {
                var results = new List<TReturn>();
                foreach (var item in funcToTime.Data)
                {
                    stopWatch.Start();
                    var result = await funcToTime.Func.Invoke(item);
                    stopWatch.Stop();
                    results.Add(result);
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
                var jobList = new List<Task>();
                stopWatch.Start();
                foreach (var item in funcToTime.Data)
                {
                    jobList.Add(funcToTime.Func.Invoke(item));
                }
                await Task.WhenAll(jobList);
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
    }
}