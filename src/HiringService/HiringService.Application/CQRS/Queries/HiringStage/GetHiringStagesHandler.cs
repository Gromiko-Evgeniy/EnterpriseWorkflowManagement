using HiringService.Application.Abstractions;
using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.StageQueries;

public class GetHiringStagesHandler : IRequestHandler<GetHiringStagesQuery, List<HiringStage>>
{
    private readonly IHiringStageRepository stages;

    public GetHiringStagesHandler(IHiringStageRepository repository)
    {
        stages = repository;
    }

    public async Task<List<HiringStage>> Handle(GetHiringStagesQuery request, CancellationToken cancellationToken)
    {
        return await stages.GetAllAsync();
    }
}

