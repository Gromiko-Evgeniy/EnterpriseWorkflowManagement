namespace ProjectManagementService.Application.Exceptions.Worker;

public class WorkerHasNoTaskNowException : CustomException
{
    private const string _messageText = "Worker has no task now";

    public WorkerHasNoTaskNowException() : base(_messageText) { }

    public WorkerHasNoTaskNowException(string message) : base(_messageText + " (" + message + ")") { }
}
