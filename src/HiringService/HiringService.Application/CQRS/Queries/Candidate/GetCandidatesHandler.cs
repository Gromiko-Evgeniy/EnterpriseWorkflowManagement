using HiringService.Application.Abstractions;
using HiringService.Domain.Entities;
using MediatR;
namespace HiringService.Application.CQRS.CandidateQueries;

public class GetCandidatesHandler : IRequestHandler<GetCandidatesQuery, List<Candidate>>
{
    private readonly ICandidateRepository candidates;

    public GetCandidatesHandler(ICandidateRepository repository)
    {
        candidates = repository;
    }

    public async Task<List<Candidate>> Handle(GetCandidatesQuery request, CancellationToken cancellationToken)
    {
        return await candidates.GetAllAsync();
    }
}
