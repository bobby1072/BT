using System.Diagnostics;

namespace BT.Common.OperationTimer.Models
{
    internal class FuncToTime<TParam, TReturn>
    {
        private static readonly Type _taskType = typeof(Task);
        private static bool _isReturnTypeTask = _taskType.IsAssignableFrom(typeof(TReturn));
        private static bool _isReturnTypeTaskWithResult = _taskType.IsAssignableFrom(typeof(Task<TReturn>));
        private Func<TParam, TReturn?> Func { get; init; }
        private IReadOnlyCollection<TParam> Data { get; init; }
        internal FuncToTime(Func<TParam, TReturn?> func, TParam data)
        {
            Func = func;
            Data = [data];
        }
        internal FuncToTime(Func<TParam, TReturn?> func, IEnumerable<TParam> data)
        {
            Func = func;
            Data = data.ToArray();
        }
        internal (TimeSpan TimeTaken, IReadOnlyCollection<object?>? Result) RunWithResult()
        {
            var stopWatch = new Stopwatch();
            var resultsList = new List<object?>();
            foreach (var item in Data)
            {
                if (_isReturnTypeTaskWithResult)
                {
                    stopWatch.Start();
                    var funcWithReturn = (Func.Invoke(item) as Task<TReturn>) ?? throw new InvalidOperationException("Func must be a Func<TParam, Task<TReturn>> to return a Task result.");
                    var result = funcWithReturn.GetAwaiter().GetResult();
                    stopWatch.Stop();
                    resultsList.Add(result);
                }
                else if (_isReturnTypeTask)
                {
                    stopWatch.Start();
                    var result = Func.Invoke(item) as Task ?? throw new InvalidOperationException("Func must be a Func<TParam, Task> to return a Task result.");
                    result.GetAwaiter().GetResult();
                    stopWatch.Stop();
                }
                else
                {
                    stopWatch.Start();
                    var result = Func.Invoke(item);
                    stopWatch.Stop();
                    resultsList.Add(result);
                }
            }
            var resultsArray = resultsList.ToArray() as object[];
            return (stopWatch.Elapsed, resultsArray?.Length > 0 ? resultsArray : null);
        }
        internal TimeSpan Run()
        {

            return RunWithResult().TimeTaken;
        }
        internal async Task<(TimeSpan TimeTaken, IReadOnlyCollection<object?>? Result)> RunWithResultAsync(bool awaitAllAtOnce = false)
        {
            var stopWatch = new Stopwatch();
            if (_isReturnTypeTaskWithResult)
            {
                if (awaitAllAtOnce)
                {
                    stopWatch.Start();
                    var jobLists = Data.Select(item => (Func.Invoke(item) as Task<TReturn>) ?? throw new InvalidOperationException("Func must be a Func<TParam, Task<TReturn>> to return a Task result."));
                    await Task.WhenAll(jobLists);
                    stopWatch.Stop();
                    var resultsArray = jobLists.Select(x => x.Result).ToArray() as object[];
                    return (stopWatch.Elapsed, resultsArray?.Length > 0 ? resultsArray : null);
                }
                else
                {
                    var results = new List<object?>();
                    foreach (var item in Data)
                    {
                        stopWatch.Start();
                        var result = await ((Func.Invoke(item) as Task<TReturn>) ?? throw new InvalidOperationException("Func must be a Func<TParam, Task<TReturn>> to return a Task result."));
                        stopWatch.Stop();
                        results.Add(result);
                    }
                    var resultsArray = results.ToArray();
                    return (stopWatch.Elapsed, resultsArray.Length > 0 ? resultsArray : null);
                }
            }
            else if (_isReturnTypeTask)
            {
                if (awaitAllAtOnce)
                {
                    stopWatch.Start();
                    var jobLists = Data.Select(item => Func.Invoke(item) as Task ?? throw new InvalidOperationException("Func must be a Func<TParam, Task> to return a Task result."));
                    await Task.WhenAll(jobLists);
                    stopWatch.Stop();
                }
                else
                {
                    foreach (var item in Data)
                    {
                        stopWatch.Start();
                        var result = Func.Invoke(item);
                        stopWatch.Stop();
                    }
                }
                return (stopWatch.Elapsed, null);
            }
            else
            {
                return RunWithResult();
            }

        }
        internal async Task<TimeSpan> RunAsync(bool awaitAllAtOnce = false)
        {
            return (await RunWithResultAsync(awaitAllAtOnce)).TimeTaken;
        }
    }
}