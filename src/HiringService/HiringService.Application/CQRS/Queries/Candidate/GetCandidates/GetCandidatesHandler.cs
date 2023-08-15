using HiringService.Application.Abstractions;
using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.CandidateQueries;

public class GetCandidatesHandler : IRequestHandler<GetCandidatesQuery, List<Candidate>>
{
    private readonly ICandidateRepository _candidateRepository;

    public GetCandidatesHandler(ICandidateRepository candidateRepository)
    {
        _candidateRepository = candidateRepository;
    }

    public async Task<List<Candidate>> Handle(GetCandidatesQuery request, CancellationToken cancellationToken)
    {
        return await _candidateRepository.GetAllAsync();
    }
}
