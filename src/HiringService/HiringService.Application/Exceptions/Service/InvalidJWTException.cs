namespace HiringService.Application.Exceptions.Service;

internal class InvalidJWTException : CustomException
{
    private const string _messageText = "This JWT is invalid";

    public InvalidJWTException() : base(_messageText) { }

    public InvalidJWTException(string message) : base(_messageText + " (" + message + ")") { }
}