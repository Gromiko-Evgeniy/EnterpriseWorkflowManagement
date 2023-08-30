﻿namespace IdentityService.Application.Exceptions.Candidate;

public class CandidateAlreadyExistsException : CustomException
{
    private const string _messageText = "Candidate with such e-mail already exists";

    public CandidateAlreadyExistsException() : base(_messageText) { }

    public CandidateAlreadyExistsException(string message) : base(_messageText + " (" + message + ")") { }
}
