namespace BT.Common.Http.Exceptions;

internal class HttpRequestBuilderException: Exception
{
    public HttpRequestBuilderException(string message) : base(message) {}
    public HttpRequestBuilderException(string message, Exception innerException) : base(message, innerException) {}
}