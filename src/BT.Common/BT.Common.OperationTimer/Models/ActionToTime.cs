using System.Diagnostics;

namespace BT.Common.OperationTimer.Models
{
    internal class ActionToTime<TParam>(Action<TParam> action, IReadOnlyCollection<TParam> data) : IOperationTimerObject
    {
        public Action<TParam> Action { get; init; } = action;
        public IReadOnlyCollection<TParam> Data { get; init; } = data;
        public ActionToTime(Action<TParam> action, TParam data) : this(action, [data]) { }
        public TimeSpan Run()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            foreach (var item in Data)
            {
                Action.Invoke(item);
            }
            stopWatch.Stop();
            return stopWatch.Elapsed;
        }
        public Task<TimeSpan> RunAsync(bool awaitAllAtOnce = false) => Task.FromResult(Run());

    }
    internal class ActionToTime(Action action) : IOperationTimerObject
    {
        public Action Action { get; init; } = action;

        public TimeSpan Run()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            Action.Invoke();
            stopWatch.Stop();
            return stopWatch.Elapsed;
        }
        public Task<TimeSpan> RunAsync(bool awaitAllAtOnce = false) => Task.FromResult(Run());
    }
}