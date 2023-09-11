using AutoMapper;
using HiringService.Application.Abstractions.RepositoryAbstractions;
using HiringService.Application.Cache;
using HiringService.Application.DTOs.CandidateDTOs;
using HiringService.Application.Exceptions.Candidate;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace HiringService.Application.CQRS.CandidateQueries;

public class GetCandidateByEmailHandler : IRequestHandler<GetCandidateByEmailQuery, CandidateMainInfoDTO>
{
    private readonly ICandidateRepository _candidateRepository;
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;

    public GetCandidateByEmailHandler(ICandidateRepository candidateRepository,
        IMapper mapper, IDistributedCache cache)
    {
        _candidateRepository = candidateRepository;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<CandidateMainInfoDTO> Handle(GetCandidateByEmailQuery request, CancellationToken cancellationToken)
    {
        var emailKey = "Candidate_" + request.Email;

        var cachedCandidate = await _cache.GetRecordAsync<CandidateMainInfoDTO>(emailKey);
        if (cachedCandidate is not null) return cachedCandidate;

        var candidate = await _candidateRepository.GetByEmailAsync(request.Email);
        if (candidate is null) throw new NoCandidateWithSuchEmailException();

        var candidateDTO = _mapper.Map<CandidateMainInfoDTO>(candidate);

        var idKey = "Candidate_" + candidateDTO.Id;

        await _cache.SetRecordAsync(emailKey, candidateDTO);
        await _cache.SetRecordAsync(idKey, candidateDTO);

        return candidateDTO;
    }
}
