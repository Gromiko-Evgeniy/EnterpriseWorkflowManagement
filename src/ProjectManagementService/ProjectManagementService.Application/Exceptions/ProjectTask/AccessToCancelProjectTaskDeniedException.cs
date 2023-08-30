namespace ProjectManagementService.Application.Exceptions.ProjectTask;

public class AccessToCancelProjectTaskDeniedException : CustomException
{
    private const string messageText = "Access to project task denied. " +
        "Project task can be canceled only by it's owner (customer)";

    public AccessToCancelProjectTaskDeniedException() : base(messageText) { }

    public AccessToCancelProjectTaskDeniedException(string message) : base(messageText + " (" + message + ")") { }
}
