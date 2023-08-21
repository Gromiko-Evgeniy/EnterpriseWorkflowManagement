using IdentityService.Application.Exceptions;

namespace ProjectManagementService.Application.Exceptions.ProjectTask;

public class NoProjectTaskWithSuchIdException : CustomException
{
    private const string _messageText = "Project task name with such id not found";

    public NoProjectTaskWithSuchIdException() : base(_messageText) { }

    public NoProjectTaskWithSuchIdException(string message) : base(_messageText + " (" + message + ")") { }
}