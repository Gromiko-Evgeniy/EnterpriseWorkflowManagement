using HiringService.Application.Abstractions;
using HiringService.Application.Exceptions.Candidate;
using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.CandidateQueries;

public class GetCandidateByIdHandler : IRequestHandler<GetCandidateByIdQuery, Candidate>
{
    private readonly ICandidateRepository _candidateRepository;

    public GetCandidateByIdHandler(ICandidateRepository candidateRepository)
    {
        _candidateRepository = candidateRepository;
    }

    public async Task<Candidate> Handle(GetCandidateByIdQuery request, CancellationToken cancellationToken)
    {
        var candidate = await _candidateRepository.GetByIdAsync(request.Id);
        
        if (candidate is null) throw new NoCandidateWithSuchIdException();

        return candidate;
    }
}