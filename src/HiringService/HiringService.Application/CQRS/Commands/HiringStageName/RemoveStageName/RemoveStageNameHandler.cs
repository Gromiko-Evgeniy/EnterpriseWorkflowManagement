using HiringService.Application.Abstractions;
using HiringService.Application.Exceptions.HiringStageName;
using MediatR;

namespace HiringService.Application.CQRS.StageNameCommands;

public class RemoveStageNameHandler : IRequestHandler<RemoveStageNameCommand>
{
    private readonly IHiringStageNameRepository _nameRepository;

    public RemoveStageNameHandler(IHiringStageNameRepository nameRepository)
    {
        _nameRepository = nameRepository;
    }

    public async Task<Unit> Handle(RemoveStageNameCommand request, CancellationToken cancellationToken)
    {
        var stageName = await _nameRepository.GetByIdAsync(request.Id);

        if (stageName is null) throw new NoStageNameWithSuchIdException();

        _nameRepository.RemoveAsync(stageName);

        await _nameRepository.SaveChangesAsync();

        return Unit.Value;
    }
}
