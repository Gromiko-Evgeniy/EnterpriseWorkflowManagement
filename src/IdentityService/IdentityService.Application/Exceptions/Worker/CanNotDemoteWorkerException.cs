using ProjectManagementService.Application.Exceptions;

namespace IdentityService.Application.Exceptions.Worker;

public class CanNotDemoteWorkerException : CustomException
{
    private const string _messageText = "Can not demote an employee: already the lowest position";

    public CanNotDemoteWorkerException() : base(_messageText) { }

    public CanNotDemoteWorkerException(string message) : base(_messageText + " (" + message + ")") { }
}
