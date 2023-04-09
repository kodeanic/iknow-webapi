using System.Net;

namespace Application.Common.Exceptions;

public class NotFoundException : AbstractHttpException
{
    public NotFoundException(string? message = null) : base(HttpStatusCode.NotFound, message)
    {
    }
}
