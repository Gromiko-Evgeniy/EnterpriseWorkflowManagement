using AutoMapper;
using HiringService.Application.Abstractions.RepositoryAbstractions;
using HiringService.Application.DTOs.CandidateDTOs;
using MediatR;

namespace HiringService.Application.CQRS.CandidateQueries;

public class GetCandidatesHandler : IRequestHandler<GetCandidatesQuery, List<CandidateShortInfoDTO>>
{
    private readonly ICandidateRepository _candidateRepository;
    private readonly IMapper _mapper;

    public GetCandidatesHandler(ICandidateRepository candidateRepository, IMapper mapper)
    {
        _candidateRepository = candidateRepository;
        _mapper = mapper;
    }

    public async Task<List<CandidateShortInfoDTO>> Handle(GetCandidatesQuery request, CancellationToken cancellationToken)
    {
        var candidates = await _candidateRepository.GetAllAsync();

        var candidateDtos = candidates.Select(_mapper.Map<CandidateShortInfoDTO>).ToList();

        return candidateDtos;
    }
}
