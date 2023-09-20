namespace ProjectManagementService.Application.Exceptions.Validation;

public class CustomValidationException : CustomException
{
    private const string _messageText = "Request data is invalid";

    public CustomValidationException() : base(_messageText) { }

    public CustomValidationException(string message) : base(_messageText + ": \r\n" + message) { }
}
