using HiringService.Application.Abstractions;
using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.StageNameQueries;

public class GetHiringStageNamesHandler : IRequestHandler<GetHiringStageNamesQuery, List<HiringStageName>>
{
    private readonly IHiringStageNameRepository _nameRepository;

    public GetHiringStageNamesHandler(IHiringStageNameRepository nameRepository)
    {
        _nameRepository = nameRepository;
    }

    public async Task<List<HiringStageName>> Handle(GetHiringStageNamesQuery request, CancellationToken cancellationToken)
    {
        return await _nameRepository.GetAllAsync();
    }
}

