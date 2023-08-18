namespace ProjectManagementService.Application.Exceptions.Worker;

public class NoWorkerWithSuchIdException : CustomException
{
    private const string _messageText = "Worker with such id not found";

    public NoWorkerWithSuchIdException() : base(_messageText) { }

    public NoWorkerWithSuchIdException(string message) : base(_messageText + " (" + message + ")") { }
}
