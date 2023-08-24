namespace ProjectManagementService.Application.Exceptions.Project;

public class CustomerAccessToProjecDeniedException : CustomException
{
    private const string messageText = "Access to project denied. Project can be accessed only by it's owner (customer)";

    public CustomerAccessToProjecDeniedException() : base(messageText) { }

    public CustomerAccessToProjecDeniedException(string message) : base(messageText + " (" + message + ")") { }
}
