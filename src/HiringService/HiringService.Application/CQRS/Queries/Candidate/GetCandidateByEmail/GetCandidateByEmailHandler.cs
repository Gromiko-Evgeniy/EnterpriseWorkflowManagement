using HiringService.Application.Abstractions;
using HiringService.Application.Exceptions.Candidate;
using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.CandidateQueries;

public class GetCandidateByEmailHandler : IRequestHandler<GetCandidateByEmailQuery, Candidate>
{
    private readonly ICandidateRepository _candidateRepository;

    public GetCandidateByEmailHandler(ICandidateRepository candidateRepository)
    {
        _candidateRepository = candidateRepository;
    }

    public async Task<Candidate> Handle(GetCandidateByEmailQuery request, CancellationToken cancellationToken)
    {
        var candidate = await _candidateRepository.GetByEmailAsync(request.Email);

        if (candidate is null) throw new NoCandidateWithSuchEmailException();

        return candidate;
    }
}