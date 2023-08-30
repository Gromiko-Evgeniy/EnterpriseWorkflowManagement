namespace ProjectManagementService.Application.Exceptions.Customer;

public class NoCustomerWithSuchEmailException : CustomException
{
    private const string _messageText = "Customer with such e-mail not found";

    public NoCustomerWithSuchEmailException() : base(_messageText) { }

    public NoCustomerWithSuchEmailException(string message) : base(_messageText + " (" + message + ")") { }
}
