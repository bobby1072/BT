namespace BT.Common.OperationTimer.Models
{
    internal interface IOperationTimer
    {
        public abstract TimeSpan Run();
        public abstract Task<TimeSpan> RunAsync(bool awaitAllAtOnce = false);
    }
}