namespace HiringService.Application.Exceptions.HiringStage;

public class AccessToHiringStageDeniedException : CustomException
{
    private const string messageText = "Access to hiring stage denied. Stage can be marked as passed successfully only by it's intervier";
    
    public AccessToHiringStageDeniedException() : base(messageText) { }

    public AccessToHiringStageDeniedException(string message) : base(messageText + " (" + message + ")") { }
}
