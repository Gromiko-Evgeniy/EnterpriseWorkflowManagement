namespace ProjectManagementService.Application.Exceptions.ProjectTask;

public class NoTaskWithSuchWorkerIdException : CustomException
{
    private const string _messageText = "No task with such worker id";

    public NoTaskWithSuchWorkerIdException() : base(_messageText) { }

    public NoTaskWithSuchWorkerIdException(string message) : base(_messageText + " (" + message + ")") { }
}
