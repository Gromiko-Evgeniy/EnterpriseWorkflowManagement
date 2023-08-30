namespace IdentityService.Application.Exceptions.Candidate;

public class NoCandidateWithSuchEmailException : CustomException
{
    private const string _messageText = "Customer with such e-mail not found";

    public NoCandidateWithSuchEmailException() : base(_messageText) { }

    public NoCandidateWithSuchEmailException(string message) : base(_messageText + " (" + message + ")") { }
}
