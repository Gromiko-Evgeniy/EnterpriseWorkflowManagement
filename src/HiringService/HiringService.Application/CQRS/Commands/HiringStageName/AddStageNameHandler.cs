using HiringService.Application.Abstractions;
using HiringService.Application.CQRS.StageNameCommands;
using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.HiringStageCommands;
public class AddStageNameHandler : IRequestHandler<AddStageNameCommand, int>
{
    private readonly IHiringStageNameRepository names;

    public AddStageNameHandler(IHiringStageNameRepository repository)
    {
        names = repository;
    }

    public async Task<int> Handle(AddStageNameCommand request, CancellationToken cancellationToken)
    {
        var id = await names.AddAsync(request.Name);

        return id;
    }
}

