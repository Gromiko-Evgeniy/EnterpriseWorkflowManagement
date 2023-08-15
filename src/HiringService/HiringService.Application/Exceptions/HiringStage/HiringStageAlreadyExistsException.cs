namespace HiringService.Application.Exceptions.HiringStage;

public class HiringStageAlreadyExistsException : CustomException
{
    private const string _messageText = "Hiring stage With Such e-mail already exists";

    public HiringStageAlreadyExistsException() : base(_messageText) { }

    public HiringStageAlreadyExistsException(string message) : base(_messageText + " (" + message + ")") { }
}