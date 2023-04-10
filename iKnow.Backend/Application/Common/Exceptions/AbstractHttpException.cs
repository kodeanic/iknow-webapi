using System.Net;

namespace Application.Common.Exceptions;

public abstract class AbstractHttpException : Exception
{
    public readonly int StatusCode;
    
    protected AbstractHttpException(HttpStatusCode httpCode, string? message = null) : base(message)
    {
        StatusCode = (int)httpCode;
    }
}
