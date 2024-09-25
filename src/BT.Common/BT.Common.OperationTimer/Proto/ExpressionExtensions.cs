using System.Linq.Expressions;

namespace BT.Common.OperationTimer.Proto
{
    public static class ExpressionExtensions
    {
        /// <summary>
        /// This method will asyncronously run the method and return a timespan for how long it took
        /// </summary>
        public static Task<TimeSpan> TimeAsync(this Expression<Func<Task>> expressionOfTaskFunc)
        {
            return OperationTimerUtils.TimeAsync(expressionOfTaskFunc);
        }
        /// <summary>
        /// This method will syncronously run the method and return a timespan for how long it took
        /// </summary>
        public static TimeSpan Time<T>(this Expression<Func<T>> expressionOfFunc)
        {
            return OperationTimerUtils.Time(expressionOfFunc);
        }
        /// <summary>
        /// This method will syncronously run the method and return a timespan for how long it took
        /// </summary>
        public static TimeSpan Time(this Expression<Action> expressionOfAction)
        {
            return OperationTimerUtils.Time(expressionOfAction);
        }
    }
}