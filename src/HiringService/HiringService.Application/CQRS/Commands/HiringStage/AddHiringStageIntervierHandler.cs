using HiringService.Application.Abstractions;
using MediatR;

namespace HiringService.Application.CQRS.HiringStageCommands;
public class AddHiringStageIntervierHandler : IRequestHandler<AddHiringStageIntervierCommand>
{
    private readonly IHiringStageRepository stages;

    public AddHiringStageIntervierHandler(IHiringStageRepository stageRepository)
    {
        stages = stageRepository;
    }

    public async Task<Unit> Handle(AddHiringStageIntervierCommand request, CancellationToken cancellationToken)
    {
        await stages.AddIntervierAsync(request.IntervierId, request.StageId);

        return Unit.Value;
    }
}
