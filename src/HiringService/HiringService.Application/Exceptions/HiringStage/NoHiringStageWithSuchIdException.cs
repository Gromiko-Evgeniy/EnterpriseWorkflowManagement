namespace HiringService.Application.Exceptions.HiringStage;

public class NoHiringStageWithSuchIdException : CustomException
{
    private const string _messageText = "Hiring stage With Such id not found";

    public NoHiringStageWithSuchIdException() : base(_messageText) { }

    public NoHiringStageWithSuchIdException(string message) : base(_messageText + " (" + message + ")") { }
}
