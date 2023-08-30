using IdentityService.Application.Exceptions;

namespace ProjectManagementService.Application.Exceptions.Project;

public class AccessToCancelProjectDeniedException : CustomException
{
    private const string messageText = "Access to project denied. Project can be canceled only by it's owner (customer)";

    public AccessToCancelProjectDeniedException() : base(messageText) { }

    public AccessToCancelProjectDeniedException(string message) : base(messageText + " (" + message + ")") { }
}
