using HiringService.Application.Abstractions;
using HiringService.Domain.Entities;
using MediatR;
namespace HiringService.Application.CQRS.StageNameQueries;

public class GetHiringStageNamesHandler : IRequestHandler<GetHiringStageNamesQuery, List<HiringStageName>>
{
    private readonly IHiringStageNameRepository names;

    public GetHiringStageNamesHandler(IHiringStageNameRepository repository)
    {
        names = repository;
    }

    public async Task<List<HiringStageName>> Handle(GetHiringStageNamesQuery request, CancellationToken cancellationToken)
    {
        return await names.GetAllAsync();
    }
}

