using HiringService.Application.Abstractions;
using HiringService.Application.Exceptions.Candidate;
using HiringService.Application.Exceptions.HiringStage;
using HiringService.Application.Exceptions.HiringStageName;
using MediatR;

namespace HiringService.Application.CQRS.HiringStageCommands;

public class MarkStageAsPassedSuccessfullyHandler : IRequestHandler<MarkStageAsPassedSuccessfullyCommand>
{
    private readonly IHiringStageRepository _stageRepository;
    private readonly IWorkerRepository _workerRepository;

    public MarkStageAsPassedSuccessfullyHandler(IHiringStageRepository stageRepository, IWorkerRepository workerRepository)
    {
        _stageRepository = stageRepository;
        _workerRepository = workerRepository;
    }

    public async Task<Unit> Handle(MarkStageAsPassedSuccessfullyCommand request, CancellationToken cancellationToken)
    {
        var worker = await _workerRepository.GetByIdAsync(request.IntervierId);
        var stage = await _stageRepository.GetByIdAsync(request.StageId);

        if (worker is null) throw new NoWorkerWithSuchIdException();
        if (stage is null) throw new NoStageNameWithSuchIdException();

        if (stage.IntervierId != request.IntervierId) throw new AccessToHiringStageDeniedException();

        stage.PassedSuccessfully = true;

        await _stageRepository.UpdateAsync(stage);

        return Unit.Value;
    }
}
