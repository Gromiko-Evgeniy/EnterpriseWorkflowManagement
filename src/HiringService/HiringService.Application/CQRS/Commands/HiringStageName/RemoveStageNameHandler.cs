using HiringService.Application.Abstractions;
using HiringService.Application.CQRS.StageNameCommands;
using MediatR;

namespace HiringService.Application.CQRS.HiringStageCommands;
public class RemoveStageNameHandler : IRequestHandler<RemoveStageNameCommand>
{
    private readonly IHiringStageNameRepository names;

    public RemoveStageNameHandler(IHiringStageNameRepository repository)
    {
        names = repository;
    }

    public async Task<Unit> Handle(RemoveStageNameCommand request, CancellationToken cancellationToken)
    {
        await names.RemoveAsync(request.Id);

        return Unit.Value;
    }
}
