namespace Mgr.Core.Exceptions;
public class UnauthorizedException : CustomException
{
    public UnauthorizedException(string message)
       : base(message, null, System.Net.HttpStatusCode.Unauthorized)
    {
    }
}

