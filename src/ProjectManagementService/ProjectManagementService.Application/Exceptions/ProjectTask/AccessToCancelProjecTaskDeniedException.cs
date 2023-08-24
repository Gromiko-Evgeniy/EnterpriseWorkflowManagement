namespace ProjectManagementService.Application.Exceptions.ProjectTask;

public class AccessToCancelProjecTaskDeniedException : CustomException
{
    private const string messageText = "Access to project task denied. " +
        "Project task can be canceled only by it's owner (customer)";

    public AccessToCancelProjecTaskDeniedException() : base(messageText) { }

    public AccessToCancelProjecTaskDeniedException(string message) : base(messageText + " (" + message + ")") { }
}
