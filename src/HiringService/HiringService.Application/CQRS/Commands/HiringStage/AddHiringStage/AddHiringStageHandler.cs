using AutoMapper;
using HiringService.Application.Abstractions.RepositoryAbstractions;
using HiringService.Application.Cache;
using HiringService.Application.DTOs.HiringStageDTOs;
using HiringService.Application.Exceptions.Candidate;
using HiringService.Application.Exceptions.HiringStageName;
using HiringService.Application.Exceptions.Worker;
using HiringService.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace HiringService.Application.CQRS.HiringStageCommands;

public class AddHiringStageHandler : IRequestHandler<AddHiringStageCommand, int>
{
    private readonly IHiringStageRepository _stageRepository;
    private readonly IHiringStageNameRepository _nameRepository;
    private readonly ICandidateRepository _candidateRepository;
    private readonly IWorkerRepository _workerRepository;
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;

    public AddHiringStageHandler(IHiringStageRepository stageRepository,
        IWorkerRepository workerRepository, IMapper mapper,
        ICandidateRepository candidateRepository, IDistributedCache cache,
        IHiringStageNameRepository nameRepository)
    {
        _candidateRepository = candidateRepository;
        _stageRepository = stageRepository;
        _nameRepository = nameRepository;
        _workerRepository = workerRepository;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<int> Handle(AddHiringStageCommand request, CancellationToken cancellationToken)
    {
        var stageDTO = request.StageDTO;

        var stageName = await _nameRepository.GetByIdAsync(stageDTO.HiringStageNameId);
        var candidate = await _candidateRepository.GetByIdAsync(stageDTO.CandidateId);
        var intervier = await _workerRepository.GetByIdAsync(stageDTO.IntervierId);

        if (stageName is null) throw new NoStageNameWithSuchIdException();
        if (candidate is null) throw new NoCandidateWithSuchIdException();
        if (intervier is null) throw new NoWorkerWithSuchIdException();

        var stage = _mapper.Map<HiringStage>(stageDTO);

        stage = _stageRepository.Add(stage);
        await _stageRepository.SaveChangesAsync();

        var idKey = "HiringStage_" + stage.Id;
        var hiringStageDTO = _mapper.Map<HiringStageMainInfoDTO>(stage);

        await _cache.SetRecordAsync(idKey, hiringStageDTO);

        return stage.Id;
    }
}
