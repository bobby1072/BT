using System.Diagnostics;
using BT.Common.OperationTimer.Models;

namespace BT.Common.OperationTimer.Common
{
    internal static class FuncToTimeExtensions
    {
        internal static (TimeSpan TimeTaken, IReadOnlyCollection<TReturn> Result) Execute<TParam, TReturn>(this FuncToTime<TParam, Task<TReturn>> funcToTime)
        {
            try
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
            catch (Exception e)
            {
                throw new OperationTimerException(e);
            }
        }
        internal static (TimeSpan TimeTaken, IReadOnlyCollection<Task> Result) Execute<TParam>(this FuncToTime<TParam, Task> funcToTime)
        {
            try
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
            catch (Exception e)
            {
                throw new OperationTimerException(e);
            }
        }
        internal static (TimeSpan TimeTaken, IReadOnlyCollection<TReturn> Result) Execute<TParam, TReturn>(this FuncToTime<TParam, TReturn> funcToTime)
        {
            try
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
            catch (Exception e)
            {
                throw new OperationTimerException(e);
            }
        }
        internal static async Task<(TimeSpan TimeTaken, IReadOnlyCollection<TReturn> Result)> ExecuteAsync<TParam, TReturn>(this FuncToTime<TParam, Task<TReturn>> funcToTime, bool awaitAllAtOnce = false)
        {
            try
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
            catch (Exception e)
            {
                throw new OperationTimerException(e);
            }
        }
        internal static async Task<(TimeSpan TimeTaken, IReadOnlyCollection<Task> Result)> ExecuteAsync<TParam>(this FuncToTime<TParam, Task> funcToTime, bool awaitAllAtOnce = false)
        {
            try
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
            catch (Exception e)
            {
                throw new OperationTimerException(e);
            }
        }
        internal static async Task<(TimeSpan TimeTaken, IReadOnlyCollection<TReturn> Result)> ExecuteAsync<TParam, TReturn>(this FuncToTime<TParam, ValueTask<TReturn>> funcToTime)
        {
            try
            {
                var stopWatch = new Stopwatch();

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
            catch (Exception e)
            {
                throw new OperationTimerException(e);
            }
        }
        internal static async Task<(TimeSpan TimeTaken, IReadOnlyCollection<ValueTask> Result)> ExecuteAsync<TParam>(this FuncToTime<TParam, ValueTask> funcToTime)
        {
            try
            {
                var stopWatch = new Stopwatch();
                foreach (var item in funcToTime.Data)
                {
                    stopWatch.Start();
                    await funcToTime.Func.Invoke(item);
                    stopWatch.Stop();
                }
                return (stopWatch.Elapsed, funcToTime.Data.Select(item => ValueTask.CompletedTask).ToArray());
            }
            catch (Exception e)
            {
                throw new OperationTimerException(e);
            }
        }
    }
}