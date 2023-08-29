namespace HiringService.Application.Exceptions.Candidate;

public class NoCandidateWithSuchIdException : CustomException
{
    private const string _messageText = "Candidate with such id not found";

    public NoCandidateWithSuchIdException() : base(_messageText) { }

    public NoCandidateWithSuchIdException(string message) : base(_messageText + " (" + message + ")") { }
}
