using HiringService.Application.Abstractions;
using HiringService.Application.Exceptions.Candidate;
using HiringService.Application.Exceptions.HiringStageName;
using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.HiringStageCommands;

public class AddHiringStageHandler : IRequestHandler<AddHiringStageCommand, int>
{
    private readonly IHiringStageRepository _stageRepository;
    private readonly IHiringStageNameRepository _nameRepository;
    private readonly ICandidateRepository _candidateRepository;

    public AddHiringStageHandler(IHiringStageRepository stageRepository,
        ICandidateRepository candidateRepository, IHiringStageNameRepository nameRepository)
    {
        _candidateRepository = candidateRepository;
        _stageRepository = stageRepository;
        _nameRepository = nameRepository;
    }

    public async Task<int> Handle(AddHiringStageCommand request, CancellationToken cancellationToken)
    {
        var stageDTO = request.StageDTO;

        var stageName = await _nameRepository.GetByIdAsync(stageDTO.HiringStageNameId);
        var candidate = await _candidateRepository.GetByIdAsync(stageDTO.CandidateId);

        if (stageName is null) throw new NoStageNameWithSuchIdException();
        if (candidate is null) throw new NoCandidateWithSuchIdException();

        var stage = new HiringStage()
        {
            HiringStageName = stageName,
            Candidate = candidate
        };

        stage = _stageRepository.AddAsync(stage);

        await _stageRepository.SaveChangesAsync();

        return stage.Id;
    }
}