namespace ProjectManagementService.Application.Exceptions.Worker;
public class NoWorkerWithSuchTaskIdException : CustomException
{
    private const string _messageText = "No worker with such task id";

    public NoWorkerWithSuchTaskIdException() : base(_messageText) { }

    public NoWorkerWithSuchTaskIdException(string message) : base(_messageText + " (" + message + ")") { }
}
