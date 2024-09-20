using System.Diagnostics;

namespace BT.Common.OperationTimer.Models
{
    internal abstract class FuncToTimeBase<TReturn>
    {
        private static readonly Type _taskType = typeof(Task);
        protected static bool IsReturnTypeTask = _taskType.IsAssignableFrom(typeof(TReturn));
    }
    internal class FuncToTime<TParam, TReturn>(Func<TParam, TReturn> func, IReadOnlyCollection<TParam> data) : FuncToTimeBase<TReturn>, IOperationTimer
    {
        public Func<TParam, TReturn> Func { get; init; } = func;
        public IReadOnlyCollection<TParam> Data { get; init; } = data;
        public FuncToTime(Func<TParam, TReturn> func, TParam data) : this(func, [data]) { }
        public TimeSpan Run()
        {
            var stopWatch = new Stopwatch();
            if (IsReturnTypeTask)
            {
                stopWatch.Start();
                foreach (var item in Data)
                {
                    var result = Func.Invoke(item) as Task ?? throw new InvalidOperationException("Func must be a Func<TParam, Task> to return a Task result.");
                    result.GetAwaiter().GetResult();
                }
                stopWatch.Stop();

            }
            else
            {
                stopWatch.Start();
                foreach (var item in Data)
                {
                    Func.Invoke(item);
                }
                stopWatch.Stop();
            }
            return stopWatch.Elapsed;
        }
        public async Task<TimeSpan> RunAsync(bool awaitAllAtOnce = false)
        {
            var stopWatch = new Stopwatch();
            if (IsReturnTypeTask)
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
                    stopWatch.Start();
                    foreach (var item in Data)
                    {
                        var result = Func.Invoke(item);
                    }
                    stopWatch.Stop();
                }
            }
            else
            {
                stopWatch.Start();
                foreach (var item in Data)
                {
                    Func.Invoke(item);
                }
                stopWatch.Stop();
            }
            return stopWatch.Elapsed;
        }
    }
    internal class FuncToTime<TReturn>(Func<TReturn> func) : FuncToTimeBase<TReturn>, IOperationTimer
    {
        public Func<TReturn> Func { get; init; } = func;
        public TimeSpan Run()
        {
            var stopWatch = new Stopwatch();
            if (IsReturnTypeTask)
            {
                stopWatch.Start();
                var result = Func.Invoke() as Task ?? throw new InvalidOperationException("Func must be a Func<Task> to return a Task result.");
                result.GetAwaiter().GetResult();
                stopWatch.Stop();
            }
            else
            {
                stopWatch.Start();
                Func.Invoke();
                stopWatch.Stop();
            }
            return stopWatch.Elapsed;
        }

        public async Task<TimeSpan> RunAsync(bool awaitAllAtOnce = false)
        {
            var stopWatch = new Stopwatch();
            if (IsReturnTypeTask)
            {
                stopWatch.Start();
                var result = Func.Invoke() as Task ?? throw new InvalidOperationException("Func must be a Func<Task> to return a Task result.");
                await result;
                stopWatch.Stop();
            }
            else
            {
                stopWatch.Start();
                Func.Invoke();
                stopWatch.Stop();
            }
            return stopWatch.Elapsed;
        }
    }

}