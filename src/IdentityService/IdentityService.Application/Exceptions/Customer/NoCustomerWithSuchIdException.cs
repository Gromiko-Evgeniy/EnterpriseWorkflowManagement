namespace IdentityService.Application.Exceptions.Customer;

public class NoCustomerWithSuchIdException : CustomException
{
    private const string _messageText = "Customer with such id not found";

    public NoCustomerWithSuchIdException() : base(_messageText) { }

    public NoCustomerWithSuchIdException(string message) : base(_messageText + " (" + message + ")") { }
}
