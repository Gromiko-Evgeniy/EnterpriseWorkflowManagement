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

public class RemoveStageNameHandler : IRequestHandler<RemoveStageNameCommand>
{
    private readonly IHiringStageNameRepository _nameRepository;
    private readonly IHiringStageRepository _stageRepository;
    private readonly ICandidateRepository _candidateRepository;
    private readonly IGRPCService _gRPCService;
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;

    public RemoveStageNameHandler(
        IHiringStageNameRepository nameRepository,
        IHiringStageRepository stageRepository,
        ICandidateRepository candidateRepository,
        IDistributedCache cache,
        IGRPCService gRPCService,
        IMapper mapper)
    {
        _nameRepository = nameRepository;
        _stageRepository = stageRepository;
        _candidateRepository = candidateRepository;
        _gRPCService = gRPCService;
        _cache = cache;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(RemoveStageNameCommand request, CancellationToken cancellationToken)
    {
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

        if (stageNamesToUpdate.Count() == 0) // no more stages need to be passed, candidates hired = become workers
        {
            await RemoveStagesAndTheirCandidatesAsync(hiringStagesToUpdate);
        }
        else
        {
            var newStageName = stageNamesToUpdate.FirstOrDefault(n => n.Index == stageName.Index + 1);

            await SetNextStageNamesAsync(hiringStagesToUpdate, newStageName!);
        }

        _nameRepository.Remove(stageName);
        await _nameRepository.SaveChangesAsync();

        if (stageNamesToUpdate.Count() > 0) 
        {
            await ShiftHiringStageNametIndexesAsync(stageNamesToUpdate);
        }

        return Unit.Value;
    }

    private async Task RemoveStagesAndTheirCandidatesAsync(List<HiringStage> hiringStages)
    {
        var candidatesToDelete = new List<Candidate>();

        foreach (var stage in hiringStages)
        {
            var candidate = await _candidateRepository.GetByIdAsync(stage.CandidateId);

            if (candidate is not null)
            {
                await _gRPCService.DeleteCandidate(candidate.Email, candidate.Name); // remotely

                candidatesToDelete.Add(candidate);
            }
        }

        _candidateRepository.RemoveRange(candidatesToDelete); // locally
        await _candidateRepository.SaveChangesAsync();
        // stages will be deleted because of cascade delete
    }

    private async Task SetNextStageNamesAsync(List<HiringStage> hiringStages, HiringStageName newStageName)
    {
        foreach (var stage in hiringStages)
        {
            if (stage.PassedSuccessfully)
            {
                _stageRepository.Remove(stage);
            }
            else
            {
                stage.HiringStageName = newStageName;
                stage.HiringStageNameId = newStageName.Id;

                _stageRepository.Update(stage);
            }
        }
        await _stageRepository.SaveChangesAsync();
    }

    private async Task ShiftHiringStageNametIndexesAsync(List<HiringStageName> stageNames)
    {
        // shifting the indices of all subsequent elements in the list
        foreach (var sName in stageNames)
        {
            sName.Index -= 1;
            _nameRepository.Update(sName);
        }
        await _nameRepository.SaveChangesAsync();
    }
}