namespace ProjectManagementService.Application.Exceptions.Project;

public class NoProjectWithSuchProjectLeaderException : CustomException
{
    private const string _messageText = "Project with such project leader not found";

    public NoProjectWithSuchProjectLeaderException() : base(_messageText) { }

    public NoProjectWithSuchProjectLeaderException(string message) : base(_messageText + " (" + message + ")") { }
}
