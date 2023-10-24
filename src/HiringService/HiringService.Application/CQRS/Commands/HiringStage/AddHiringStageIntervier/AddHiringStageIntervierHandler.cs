using HiringService.Application.Abstractions.RepositoryAbstractions;
using HiringService.Application.Exceptions.HiringStageName;
using HiringService.Application.Exceptions.Worker;
using MediatR;

namespace HiringService.Application.CQRS.HiringStageCommands;

public class AddHiringStageIntervierHandler : IRequestHandler<AddHiringStageIntervierCommand>
{
    private readonly IHiringStageRepository _stageRepository;
    private readonly IWorkerRepository _workerRepository;

    public AddHiringStageIntervierHandler(IHiringStageRepository stageRepository, IWorkerRepository workerRepository)
    {
        _stageRepository = stageRepository;
        _workerRepository = workerRepository;
    }

    public async Task<Unit> Handle(AddHiringStageIntervierCommand request, CancellationToken cancellationToken)
    {
        var worker = await _workerRepository.GetByIdAsync(request.IntervierId);
        var stage = await _stageRepository.GetByIdAsync(request.StageId);

        if (worker is null) throw new NoWorkerWithSuchIdException();
        if (stage is null) throw new NoStageNameWithSuchIdException();

        stage.Interviewer = worker;

        _stageRepository.Update(stage);
        await _stageRepository.SaveChangesAsync();

        return Unit.Value;
    }
}
