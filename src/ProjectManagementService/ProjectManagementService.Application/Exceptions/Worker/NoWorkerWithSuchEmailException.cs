namespace ProjectManagementService.Application.Exceptions.Worker;

public class NoWorkerWithSuchEmailException : CustomException
{
    private const string _messageText = "Worker with such email not found";

    public NoWorkerWithSuchEmailException() : base(_messageText) { }

    public NoWorkerWithSuchEmailException(string message) : base(_messageText + " (" + message + ")") { }
}

