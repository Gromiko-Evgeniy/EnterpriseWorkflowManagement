namespace IdentityService.Application.Exceptions.Customer;

public class CustomerAlreadyExistsException : CustomException
{
    private const string _messageText = "Customer with such e-mail already exists";

    public CustomerAlreadyExistsException() : base(_messageText) { }

    public CustomerAlreadyExistsException(string message) : base(_messageText + " (" + message + ")") { }
}