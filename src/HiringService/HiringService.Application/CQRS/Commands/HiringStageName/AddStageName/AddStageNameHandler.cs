using HiringService.Application.Abstractions;
using HiringService.Application.Exceptions.HiringStageName;
using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.StageNameCommands;

public class AddStageNameHandler : IRequestHandler<AddStageNameCommand, int>
{
    private readonly IHiringStageNameRepository _nameRepository;

    public AddStageNameHandler(IHiringStageNameRepository nameRepository)
    {
        _nameRepository = nameRepository;
    }

    public async Task<int> Handle(AddStageNameCommand request, CancellationToken cancellationToken)
    {
        var OldStageName = await _nameRepository.GetByNameAsync(request.Name);

        if (OldStageName is not null) throw new StageNameAlreadyExistsException();

        var newtageName = new HiringStageName() { Name = request.Name };

        var id = await _nameRepository.AddAsync(newtageName);

        return id;
    }
}

