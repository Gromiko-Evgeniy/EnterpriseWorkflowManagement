using AutoMapper;
using HiringService.Application.Abstractions.RepositoryAbstractions;
using HiringService.Application.Abstractions.ServiceAbstractions;
using HiringService.Application.Cache;
using HiringService.Application.DTOs.HiringStageDTOs;
using HiringService.Application.Exceptions.HiringStage;
using HiringService.Application.Exceptions.HiringStageName;
using HiringService.Application.Exceptions.Worker;
using HiringService.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace HiringService.Application.CQRS.HiringStageCommands;

public class MarkStageAsPassedSuccessfullyHandler : IRequestHandler<MarkStageAsPassedSuccessfullyCommand, string>
{
    private readonly IHiringStageRepository _stageRepository;
    private readonly IWorkerRepository _workerRepository;
    private readonly IHiringStageNameRepository _stageNameRepository;
    private readonly ICandidateRepository _candidateRepository;
    private readonly IGRPCService _gRPCService;
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;

    public MarkStageAsPassedSuccessfullyHandler(IHiringStageRepository stageRepository,
        IWorkerRepository workerRepository, IHiringStageNameRepository stageNameRepository,
        ICandidateRepository candidateRepository, IGRPCService gRPCService,
        IDistributedCache cache, IMapper mapper)
    {
        _stageRepository = stageRepository;
        _workerRepository = workerRepository;
        _stageNameRepository = stageNameRepository;
        _candidateRepository = candidateRepository;
        _gRPCService = gRPCService;
        _cache = cache;
        _mapper = mapper;
    }

    public async Task<string> Handle(MarkStageAsPassedSuccessfullyCommand request, CancellationToken cancellationToken)
    {
        string? newJWT = null;

        var intervier = await _workerRepository.GetByIdAsync(request.IntervierId);
        var stage = await _stageRepository.GetByIdAsync(request.StageId);

        if (intervier is null) throw new NoWorkerWithSuchIdException();
        if (stage is null) throw new NoStageNameWithSuchIdException();

        if (stage.IntervierId != request.IntervierId) throw new AccessToHiringStageDeniedException();

        var stageNames = await _stageNameRepository.GetAllAsync();

        var currentStageName = stageNames.FirstOrDefault(n => n.Id == stage.HiringStageNameId);

        if (currentStageName is null) throw new NoHiringStageWithSuchIdException("Hiring stage has incorrect HiringStageNameId");

        if (stageNames.Any(n => n.Index > currentStageName.Index))
        {
            var nextStageName = stageNames.FirstOrDefault(n => n.Index == currentStageName.Index + 1);

            var newStage = new HiringStage()
            {
                CandidateId = stage.CandidateId,
                IntervierId = intervier.Id,
                HiringStageNameId = nextStageName!.Id
            };

            _stageRepository.Add(newStage);
            await _stageRepository.SaveChangesAsync();

            var idKey = RedisKeysPrefixes.StagePrefix + newStage.Id;
            var hiringStageDTO = _mapper.Map<HiringStageMainInfoDTO>(newStage);

            await _cache.SetRecordAsync(idKey, hiringStageDTO);
        }
        else
        {
            var candidate = await _candidateRepository.GetByIdAsync(stage.CandidateId);

            if (candidate is not null)
            {
                newJWT = await _gRPCService.DeleteCandidate(candidate.Email, candidate.Name); // remotely
                _candidateRepository.Remove(candidate); // locally

                await _candidateRepository.SaveChangesAsync();
            }
        }

        stage.PassedSuccessfully = true;

        _stageRepository.Update(stage);
        await _stageRepository.SaveChangesAsync();

        return newJWT;
    }
}
