using HiringService.Application.Abstractions;
using MediatR;

namespace HiringService.Application.CQRS.HiringStageCommands;
public class MarkStageAsPassedSuccessfullyHandler : IRequestHandler<MarkStageAsPassedSuccessfullyCommand>
{
    private readonly IHiringStageRepository stages;

    public MarkStageAsPassedSuccessfullyHandler(IHiringStageRepository stageRepository)
    {
        stages = stageRepository;
    }

    public async Task<Unit> Handle(MarkStageAsPassedSuccessfullyCommand request, CancellationToken cancellationToken)
    {
        await stages.MarkAsPassedSuccessfullyAsync(request.IntervierId, request.StageId);

        return Unit.Value;
    }
}
