namespace HiringService.Application.Exceptions.Candidate;

public class NoCandidateWithSuchEmailException : CustomException
{
    private const string _messageText = "Candidate with such e-mail not found";

    public NoCandidateWithSuchEmailException() : base(_messageText) { }

    public NoCandidateWithSuchEmailException(string message) : base(_messageText + " (" + message + ")") { }
}