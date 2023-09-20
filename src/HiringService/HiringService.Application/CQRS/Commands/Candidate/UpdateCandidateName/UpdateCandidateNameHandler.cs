using AutoMapper;
using HiringService.Application.Abstractions.RepositoryAbstractions;
using HiringService.Application.Cache;
using HiringService.Application.DTOs.CandidateDTOs;
using HiringService.Application.Exceptions.Candidate;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace HiringService.Application.CQRS.CandidateCommands;

public class UpdateCandidateNameHandler : IRequestHandler<UpdateCandidateNameCommand>
{
    private readonly ICandidateRepository _candidateRepository;
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;

    public UpdateCandidateNameHandler(ICandidateRepository candidateRepository,
        IMapper mapper, IDistributedCache cache)
    {
        _candidateRepository = candidateRepository;
        _mapper = mapper;
        _cache = cache;
    }

    async Task<Unit> IRequestHandler<UpdateCandidateNameCommand, Unit>.Handle(UpdateCandidateNameCommand request, CancellationToken cancellationToken)
    {
        var candidate = await _candidateRepository.GetByIdAsync(request.Id);

        if (candidate is null) throw new NoCandidateWithSuchIdException();

        candidate.Name = request.Name;

        _candidateRepository.Update(candidate);
        await _candidateRepository.SaveChangesAsync();

        var emailKey = RedisKeysPrefixes.CandidatePrefix + candidate.Email;
        var idKey = RedisKeysPrefixes.CandidatePrefix + candidate.Id;
        var candidateMainInfo = _mapper.Map<CandidateMainInfoDTO>(candidate);

        await _cache.SetRecordAsync(emailKey, candidateMainInfo);
        await _cache.SetRecordAsync(idKey, candidateMainInfo);

        return Unit.Value; //fake empty value
    }
}
