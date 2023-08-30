namespace IdentityService.Application.Exceptions.Worker;

public class NoWorkerWithSuchEmailException : CustomException
{
    private const string _messageText = "Worker with such e-mail not found";

    public NoWorkerWithSuchEmailException() : base(_messageText) { }

    public NoWorkerWithSuchEmailException(string message) : base(_messageText + " (" + message + ")") { }
}
