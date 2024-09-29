namespace BT.Common.OperationTimer.Common
{
    internal static class Constants
    {
        internal static class ErrorMessages
        {
            internal const string FuncMustBeFuncTask = "Func must be a Func<TParam, Task> to return a Task result.";
            internal const string FuncMustBeFuncTaskWithResult = "Func must be a Func<TParam, Task<TReturn>> to return a Task result.";
            internal const string ResultNotFound = "Result not found.";
        }
    }
}