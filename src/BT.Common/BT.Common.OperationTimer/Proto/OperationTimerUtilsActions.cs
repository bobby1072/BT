using BT.Common.OperationTimer.Common;
using BT.Common.OperationTimer.Models;

namespace BT.Common.OperationTimer.Proto
{
    public static partial class OperationTimerUtils
    {
        /// <summary>
        /// This method will syncronously run the method and return a timespan for how long it took
        /// </summary>
        public static TimeSpan Time<TParam>(Action<TParam> action, TParam data)
        {
            var actionToTime = new FuncToTime<TParam, object>(action.ToFuncWithParams(), data);

            return actionToTime.Run();
        }
        /// <summary>
        /// This method will syncronously run the method (against the all params provided) and return a timespan for how long it took
        /// </summary>
        public static TimeSpan Time<TParam>(Action<TParam> action, IEnumerable<TParam> data)
        {
            var actionToTime = new FuncToTime<TParam, object>(action.ToFuncWithParams(), data);

            return actionToTime.Run();
        }
        /// <summary>
        /// This method will syncronously run the method and return a timespan for how long it took
        /// </summary>
        public static TimeSpan Time(Action action)
        {
            var actionToTime = new FuncToTime<object, object>(action.ToFuncWithParams(), [null]);

            return actionToTime.Run();
        }
    }
}