namespace ProjectManagementService.Application.Exceptions.ProjectTask;

public class AccessToApproveProjecTaskDeniedException : CustomException
{
    private const string messageText = "Access to project task denied. " +
        "Project task can be approved only by it's project leader";

    public AccessToApproveProjecTaskDeniedException() : base(messageText) { }

    public AccessToApproveProjecTaskDeniedException(string message) : base(messageText + " (" + message + ")") { }
}
