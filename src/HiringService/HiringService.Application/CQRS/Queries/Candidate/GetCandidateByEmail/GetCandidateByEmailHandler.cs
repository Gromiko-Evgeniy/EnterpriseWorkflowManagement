using AutoMapper;
using HiringService.Application.Abstractions.RepositoryAbstractions;
using HiringService.Application.DTOs.CandidateDTOs;
using HiringService.Application.Exceptions.Candidate;
using MediatR;

namespace HiringService.Application.CQRS.CandidateQueries;

public class GetCandidateByEmailHandler : IRequestHandler<GetCandidateByEmailQuery, CandidateMainInfoDTO>
{
    private readonly ICandidateRepository _candidateRepository;
    private readonly IMapper _mapper;

    public GetCandidateByEmailHandler(ICandidateRepository candidateRepository, IMapper mapper)
    {
        _candidateRepository = candidateRepository;
        _mapper = mapper;
    }

    public async Task<CandidateMainInfoDTO> Handle(GetCandidateByEmailQuery request, CancellationToken cancellationToken)
    {
        var candidate = await _candidateRepository.GetByEmailAsync(request.Email);

        if (candidate is null) throw new NoCandidateWithSuchEmailException();

        var candidateDTO = _mapper.Map<CandidateMainInfoDTO>(candidate);

        return candidateDTO;
    }
}
