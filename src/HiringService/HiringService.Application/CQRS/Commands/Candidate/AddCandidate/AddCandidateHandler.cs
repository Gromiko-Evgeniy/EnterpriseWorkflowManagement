using AutoMapper;
using HiringService.Application.Abstractions.RepositoryAbstractions;
using HiringService.Application.Cache;
using HiringService.Application.DTOs.CandidateDTOs;
using HiringService.Application.Exceptions.Candidate;
using HiringService.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace HiringService.Application.CQRS.CandidateCommands;

public class AddCandidateHandler : IRequestHandler<AddCandidateCommand, int>
{
    private readonly ICandidateRepository _candidateRepository;
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;

    public AddCandidateHandler(ICandidateRepository candidateRepository,
        IMapper mapper, IDistributedCache cache)
    {
        _candidateRepository = candidateRepository;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<int> Handle(AddCandidateCommand request, CancellationToken cancellationToken)
    {
        var candidateDTO = request.CandidateDTO;

        var oldCandidate = await _candidateRepository.GetByEmailAsync(request.CandidateDTO.Email);
        if (oldCandidate is not null) throw new CandidateAlreadyExistsException();

        var candidate = _mapper.Map<Candidate>(candidateDTO);

        candidate = _candidateRepository.Add(candidate);
        await _candidateRepository.SaveChangesAsync();

        var emailKey = "Candidate_" + candidate.Email;
        var idKey = "Candidate_" + candidate.Id;
        var candidateMainInfo = _mapper.Map<CandidateMainInfoDTO>(candidate);

        await _cache.SetRecordAsync(emailKey, candidateMainInfo);
        await _cache.SetRecordAsync(idKey, candidateMainInfo);

        return candidate.Id;
    }
}
