using HiringService.Application.Abstractions;
using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.CandidateQueries;

public class GetCandidateByIdHandler : IRequestHandler<GetCandidateByIdQuery, Candidate>
{
    private readonly ICandidateRepository candidates;

    public GetCandidateByIdHandler(ICandidateRepository repository)
    {
        candidates = repository;
    }

    public async Task<Candidate> Handle(GetCandidateByIdQuery request, CancellationToken cancellationToken)
    {
        return await candidates.GetByIdAsync(request.Id);
    }
}