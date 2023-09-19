using AutoMapper;
using HiringService.Application.Abstractions.RepositoryAbstractions;
using HiringService.Application.Abstractions.ServiceAbstractions;
using HiringService.Application.Cache;
using HiringService.Application.DTOs.StageNameDTOs;
using HiringService.Application.Exceptions.HiringStageName;
using HiringService.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace HiringService.Application.CQRS.StageNameCommands;

public class RemoveStageNameHandler : IRequestHandler<RemoveStageNameCommand, string>
{
    private readonly IHiringStageNameRepository _nameRepository;
    private readonly ICandidateRepository _candidateRepository;
    private readonly IHiringStageRepository _stageRepository;
    private readonly IGRPCService _gRPCService;
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;

    public RemoveStageNameHandler(IHiringStageNameRepository nameRepository,
        IHiringStageRepository stageRepository, IGRPCService gRPCService,
        ICandidateRepository candidateRepository, IDistributedCache cache,
        IMapper mapper)
    {
        _candidateRepository = candidateRepository;
        _stageRepository = stageRepository;
        _nameRepository = nameRepository;
        _gRPCService = gRPCService;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<string> Handle(RemoveStageNameCommand request, CancellationToken cancellationToken)
    {
        string? newJWT = null;

        var idKey = RedisKeysPrefixes.StageNamePrefix + request.Id;
        var stageNameDTO = await _cache.GetRecordAsync<GetStageNameDTO>(idKey);
        var stageName = _mapper.Map<HiringStageName>(stageNameDTO);

        if (stageName is null) 
        {
            stageName = await _nameRepository.GetByIdAsync(request.Id);
        }

        if (stageName is null) throw new NoStageNameWithSuchIdException();

        var stageNamesToUpdate = await _nameRepository.GetFilteredAsync(n => n.Index > stageName.Index);
        var hiringStagesToUpdate = await _stageRepository.GetFilteredAsync(s => s.HiringStageNameId == stageName.Id);

        if (stageNamesToUpdate.Count == 0) // no more stages need to be passed, candidates hired = become workers
        {
            foreach (var stage in hiringStagesToUpdate)
            {
                var candidate = await _candidateRepository.GetByIdAsync(stage.CandidateId);

                if (candidate is not null)
                {
                    newJWT = await _gRPCService.DeleteCandidate(candidate.Email, candidate.Name); // remotely
                    _candidateRepository.Remove(candidate); // locally

                    await _candidateRepository.SaveChangesAsync();
                }
            }
            // stages will be deleted because of cascade delete
        }
        else
        {
            foreach (var stage in hiringStagesToUpdate)
            {
                var newStageName = stageNamesToUpdate.FirstOrDefault(n => n.Index == stageName.Index + 1);

                stage.HiringStageName = newStageName!;
                stage.HiringStageNameId = newStageName!.Id;

                _stageRepository.Update(stage);
            }
            await _stageRepository.SaveChangesAsync();
        }

        _nameRepository.Remove(stageName);
        await _nameRepository.SaveChangesAsync();

        await _cache.RemoveAsync(idKey);

        if (stageNamesToUpdate.Count > 0)
        {
            // shifting the indexes of all subsequent elements in the list
            foreach (var sName in stageNamesToUpdate)
            {
                sName.Index -= 1;
                _nameRepository.Update(sName);
            }
            await _nameRepository.SaveChangesAsync();
        }

        return newJWT;
    }
}
