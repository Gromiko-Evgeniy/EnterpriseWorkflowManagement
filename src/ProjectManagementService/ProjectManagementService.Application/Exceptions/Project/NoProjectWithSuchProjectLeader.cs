namespace ProjectManagementService.Application.Exceptions.Project;

internal class NoProjectWithSuchProjectLeader : CustomException
{
    private const string _messageText = "Project with such project leader not found";

    public NoProjectWithSuchProjectLeader() : base(_messageText) { }

    public NoProjectWithSuchProjectLeader(string message) : base(_messageText + " (" + message + ")") { }
}
