namespace ProjectManagementService.Application.Exceptions.ProjectTask;

public class StageNameAlreadyExistsException : CustomException
{
    private const string _messageText = "Such hiring stage name already exists";

    public StageNameAlreadyExistsException() : base(_messageText) { }

    public StageNameAlreadyExistsException(string message) : base(_messageText + " (" + message + ")") { }
}
