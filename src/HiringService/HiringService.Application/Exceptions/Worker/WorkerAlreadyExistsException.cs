namespace HiringService.Application.Exceptions.Worker;

public class WorkerAlreadyExistsException : CustomException
{
    private const string _messageText = "Worker With Such e-mail already exists";

    public WorkerAlreadyExistsException() : base(_messageText) { }

    public WorkerAlreadyExistsException(string message) : base(_messageText + " (" + message + ")") { }
}
