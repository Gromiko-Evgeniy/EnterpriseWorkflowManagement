namespace HiringService.Application.Exceptions.Service;

public class NoEmailInJWTException : CustomException
{
    private const string _messageText = "This JWT does not contain an email field";

    public NoEmailInJWTException() : base(_messageText) { }

    public NoEmailInJWTException(string message) : base(_messageText + " (" + message + ")") { }
}