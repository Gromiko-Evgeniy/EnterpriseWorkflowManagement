using HiringService.Application.Abstractions;
using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.HiringStageCommands;
public class AddHiringStageHandler : IRequestHandler<AddHiringStageCommand, int>
{
    private readonly IHiringStageRepository stages;
    private readonly IHiringStageNameRepository names;
    private readonly ICandidateRepository candidates;

    public AddHiringStageHandler(IHiringStageRepository stageRepository,
        ICandidateRepository candidateRepository, IHiringStageNameRepository nameRepository)
    {
        candidates = candidateRepository;
        stages = stageRepository;
        names = nameRepository;
    }

    public async Task<int> Handle(AddHiringStageCommand request, CancellationToken cancellationToken)
    {
        var stageDTO = request.StageDTO;

        var stageName = await names.GetByIdAsync(stageDTO.HiringStageNameId);
        var candidate = await candidates.GetByIdAsync(stageDTO.CandidateId);

        var stage = new HiringStage() // use automapper
        {
            HiringStageName = stageName,
            Candidate = candidate
        };

        await stages.AddAsync(stage);

        return stage.Id;
    }
}