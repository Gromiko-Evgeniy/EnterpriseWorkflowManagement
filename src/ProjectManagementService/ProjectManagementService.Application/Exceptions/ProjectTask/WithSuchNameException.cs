namespace ProjectManagementService.Application.Exceptions.ProjectTask;

public class NoStageNameWithSuchNameException : CustomException
{
    private static string _messageText = "Hiring stage name with such name not found";

    public NoStageNameWithSuchNameException() : base(_messageText) { }

    public NoStageNameWithSuchNameException(string message) : base(_messageText + " (" + message + ")") { }
}
