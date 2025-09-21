namespace API.Template.Domain.Exceptions;

public class UnauthenticatedException : Exception
{
    public UnauthenticatedException(string message) : base(message) { }

    public UnauthenticatedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}