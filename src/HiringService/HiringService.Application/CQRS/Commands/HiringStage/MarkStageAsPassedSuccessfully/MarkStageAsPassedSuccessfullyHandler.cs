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

        var stage = await GetTheHiringStageThatIsAccessibleToTheInterviewer(request.IntervierId, request.StageId);

        var stageNames = await _stageNameRepository.GetAllAsync();
        var currentStageName = stageNames.FirstOrDefault(n => n.Id == stage.HiringStageNameId);

        if (currentStageName is null) throw new NoHiringStageWithSuchIdException("Hiring stage has incorrect HiringStageNameId");

        if (stageNames.Any(n => n.Index > currentStageName.Index)) // checking if there are any hiring stage names left
        {
            var nextStageName = stageNames.FirstOrDefault(n => n.Index == currentStageName.Index + 1);

            await CreateNextHiringStageForCandidateAsync(stage.CandidateId, request.IntervierId, nextStageName!.Id);
        }
        else
        {
            newJWT = await RemoveCandidateRemotelyAndLocallyAsync(stage.CandidateId); // make the candidate an employee
        }

        stage.PassedSuccessfully = true;

        _stageRepository.Update(stage);
        await _stageRepository.SaveChangesAsync();

        return newJWT;
    }

    private async Task<HiringStage> GetTheHiringStageThatIsAccessibleToTheInterviewer(int intervierId, int stageId)
    {
        var intervier = await _workerRepository.GetByIdAsync(intervierId);
        var stage = await _stageRepository.GetByIdAsync(stageId);

        if (intervier is null) throw new NoWorkerWithSuchIdException();
        if (stage is null) throw new NoStageNameWithSuchIdException();

        if (stage.IntervierId != intervierId) throw new AccessToHiringStageDeniedException();

        return stage;
    }

    private async Task CreateNextHiringStageForCandidateAsync(int candidateId, int intervierId, int nextStageNameId)
    {
        var newStage = new HiringStage()
        {
            CandidateId = candidateId,
            IntervierId = intervierId,
            HiringStageNameId = nextStageNameId
        };

        _stageRepository.Add(newStage);
        await _stageRepository.SaveChangesAsync();
    }

    private async Task<string?> RemoveCandidateRemotelyAndLocallyAsync(int candidateId)
    {
        string? newJWT = null;
        var candidate = await _candidateRepository.GetByIdAsync(candidateId);

        if (candidate is not null)
        {
            newJWT = await _gRPCService.DeleteCandidate(candidate.Email, candidate.Name); // remotely

            _candidateRepository.Remove(candidate); // locally
            await _candidateRepository.SaveChangesAsync();
        }

        return newJWT;
    }
}
