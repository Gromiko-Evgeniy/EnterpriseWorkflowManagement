using ProjectManagementService.Application.Exceptions;

namespace IdentityService.Application.Exceptions.Worker;

public class CanNotPromoteWorkerException : CustomException
{
    private const string _messageText = "Can not promote an employee: already the highest position";

    public CanNotPromoteWorkerException() : base(_messageText) { }

    public CanNotPromoteWorkerException(string message) : base(_messageText + " (" + message + ")") { }
}
