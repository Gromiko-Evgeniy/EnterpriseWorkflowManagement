using AutoMapper;
using HiringService.Application.Abstractions.RepositoryAbstractions;
using HiringService.Application.Cache;
using HiringService.Application.DTOs.CandidateDTOs;
using HiringService.Application.Exceptions.Candidate;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace HiringService.Application.CQRS.CandidateQueries;

public class GetCandidateByIdHandler : IRequestHandler<GetCandidateByIdQuery, CandidateMainInfoDTO>
{
    private readonly ICandidateRepository _candidateRepository;
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;

    public GetCandidateByIdHandler(ICandidateRepository candidateRepository,
        IMapper mapper, IDistributedCache cache)
    {
        _candidateRepository = candidateRepository;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<CandidateMainInfoDTO> Handle(GetCandidateByIdQuery request, CancellationToken cancellationToken)
    {
        var idKey = RedisKeysPrefixes.CandidatePrefix + request.Id;

        var cachedCandidate = await _cache.GetRecordAsync<CandidateMainInfoDTO>(idKey);
        if (cachedCandidate is not null) return cachedCandidate;

        var candidate = await _candidateRepository.GetByIdAsync(request.Id);
        if (candidate is null) throw new NoCandidateWithSuchIdException();

        var candidateDTO = _mapper.Map<CandidateMainInfoDTO>(candidate);

        var emailKey = RedisKeysPrefixes.CandidatePrefix + candidateDTO.Email;

        await _cache.SetRecordAsync(emailKey, candidateDTO);
        await _cache.SetRecordAsync(idKey, candidateDTO);

        return candidateDTO;
    }
}
