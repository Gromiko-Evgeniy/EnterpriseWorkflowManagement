namespace ProjectManagementService.Application.Exceptions.ProjectTask;

public class AccessToApproveProjectTaskDeniedException : CustomException
{
    private const string messageText = "Access to project task denied. " +
        "Project task can be approved only by it's project leader";

    public AccessToApproveProjectTaskDeniedException() : base(messageText) { }

    public AccessToApproveProjectTaskDeniedException(string message) : base(messageText + " (" + message + ")") { }
}
