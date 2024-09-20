namespace BT.Common.OperationTimer.Models
{
    internal interface IOperationTimerObject
    {
        public abstract TimeSpan Run();
        public abstract Task<TimeSpan> RunAsync(bool awaitAllAtOnce = false);
    }
}