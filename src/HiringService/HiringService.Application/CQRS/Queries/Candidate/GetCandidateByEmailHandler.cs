using HiringService.Application.Abstractions;
using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.CandidateQueries;

public class GetCandidateByEmailHandler : IRequestHandler<GetCandidateByEmailQuery, Candidate>
{
    private readonly ICandidateRepository candidates;

    public GetCandidateByEmailHandler(ICandidateRepository repository)
    {
        candidates = repository;
    }

    public async Task<Candidate> Handle(GetCandidateByEmailQuery request, CancellationToken cancellationToken)
    {
        return await candidates.GetByEmailAsync(request.Email);
    }
}