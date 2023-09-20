namespace ProjectManagementService.Application.Exceptions.Service;

public class EmptyEmailInJWTException : CustomException
{
    private const string _messageText = "This JWT does contain an empty email field";

    public EmptyEmailInJWTException() : base(_messageText) { }

    public EmptyEmailInJWTException(string message) : base(_messageText + " (" + message + ")") { }
}