using HiringService.Application.Abstractions;
using HiringService.Application.Exceptions.Candidate;
using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.CandidateCommands;

public class AddCandidateHandler : IRequestHandler<AddCandidateCommand, int>
{
    private readonly ICandidateRepository _candidateRepository;

    public AddCandidateHandler(ICandidateRepository candidateRepository)
    {
        _candidateRepository = candidateRepository;
    }

    public async Task<int> Handle(AddCandidateCommand request, CancellationToken cancellationToken)
    {
        var candidateDTO = request.CandidateDTO;

        var oldCandidate = await _candidateRepository.GetByEmailAsync(request.CandidateDTO.Email);

        if (oldCandidate is not null) throw new CandidateAlreadyExistsException();

        var candidate = new Candidate() // use automapper
        {
            Name = candidateDTO.Name,
            Email = candidateDTO.Email,
            CV = candidateDTO.CV
        };

        var id = await _candidateRepository.AddAsync(candidate);

        return id;
    }
}