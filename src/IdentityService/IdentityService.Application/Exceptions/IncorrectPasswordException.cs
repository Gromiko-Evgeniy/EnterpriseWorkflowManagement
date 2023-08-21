namespace IdentityService.Application.Exceptions;

public class IncorrectPasswordException : CustomException
{
    private const string _messageText = "Password is incorrect";

    public IncorrectPasswordException() : base(_messageText) { }

    public IncorrectPasswordException(string message) : base(_messageText + " (" + message + ")") { }
}