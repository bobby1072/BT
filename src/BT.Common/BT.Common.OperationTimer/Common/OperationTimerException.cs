namespace BT.Common.OperationTimer.Common
{
    public class OperationTimerException : Exception
    {
        public OperationTimerException(string message) : base(message) { }
        public OperationTimerException(string message, Exception innerException) : base(message, innerException) { }
    }
}