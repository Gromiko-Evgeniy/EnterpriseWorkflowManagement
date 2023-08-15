namespace HiringService.Application.Exceptions.HiringStageName;

public class NoStageNameWithSuchIdException : CustomException
{
    private const string _messageText = "Hiring stage name with such id not found";

    public NoStageNameWithSuchIdException() : base(_messageText) { }

    public NoStageNameWithSuchIdException(string message) : base(_messageText + " (" + message + ")") { }
}