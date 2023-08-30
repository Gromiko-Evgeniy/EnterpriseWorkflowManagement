using IdentityService.Application.Exceptions;

namespace ProjectManagementService.Application.Exceptions.Project;

public class NoProjectWithSuchIdException : CustomException
{
    private const string _messageText = "Project with such id not found";

    public NoProjectWithSuchIdException() : base(_messageText) { }

    public NoProjectWithSuchIdException(string message) : base(_messageText + " (" + message + ")") { }
}
