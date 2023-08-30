namespace ProjectManagementService.Application.Exceptions.Project;

public class CustomerAccessToProjectDeniedException : CustomException
{
    private const string messageText = "Access to project denied. Project can be accessed only by it's owner (customer)";

    public CustomerAccessToProjectDeniedException() : base(messageText) { }

    public CustomerAccessToProjectDeniedException(string message) : base(messageText + " (" + message + ")") { }
}
