using System.Diagnostics;
using System.Linq.Expressions;

namespace BT.Common.OperationTimer.Proto
{
    public static partial class OperationTimerUtils
    {
        /// <summary>
        /// This method will asyncronously run the method and return a timespan for how long it took
        /// </summary>
        public static async Task<TimeSpan> TimeAsync(Expression<Func<Task>> expressionOfTaskFunc)
        {
            var compiledFunc = expressionOfTaskFunc.Compile();
            var timer = new Stopwatch();
            timer.Start();
            await compiledFunc.Invoke();
            timer.Stop();
            return timer.Elapsed;
        }
        /// <summary>
        /// This method will syncronously run the method and return a timespan for how long it took
        /// </summary>
        public static TimeSpan Time<T>(Expression<Func<T>> expressionOfFunc)
        {
            var compiledFunc = expressionOfFunc.Compile();
            var timer = new Stopwatch();
            timer.Start();
            compiledFunc.Invoke();
            timer.Stop();
            return timer.Elapsed;
        }
        /// <summary>
        /// This method will syncronously run the method and return a timespan for how long it took
        /// </summary>
        public static TimeSpan Time(Expression<Action> expressionOfAction)
        {
            var compiledFunc = expressionOfAction.Compile();
            var timer = new Stopwatch();
            timer.Start();
            compiledFunc.Invoke();
            timer.Stop();
            return timer.Elapsed;
        }
    }
}