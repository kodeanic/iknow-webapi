using System.Net;

namespace Application.Common.Exceptions;

public class BadRequestException : AbstractHttpException
{
    public BadRequestException(string? message = null) : base(HttpStatusCode.BadRequest, message)
    {
    }
}