using HiringService.Application.Abstractions;
using HiringService.Application.CQRS.CandidateCommands;
using HiringService.Domain.Entities;
using MediatR;

namespace ProjectManagementService.Application.CQRS.ProjectCommands;
public class AddCandidateHandler : IRequestHandler<AddCandidateCommand, int>
{
    private readonly ICandidateRepository candidates;

    public AddCandidateHandler(ICandidateRepository candidateRepository)
    {
        candidates = candidateRepository;
    }

    public async Task<int> Handle(AddCandidateCommand request, CancellationToken cancellationToken)
    {
        var candidateDTO = request.CandidateDTO;
        
        var candidate = new Candidate() // use automapper
        {
            Name = candidateDTO.Name,
            Email = candidateDTO.Email,
            CV = candidateDTO.CV
        };

        await candidates.AddAsync(candidate);

        return candidate.Id;
    }
}