using HiringService.Application.Abstractions;
using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.StageQueries;

public class GetHiringStageByIntervierIdHandler : IRequestHandler<GetHiringStageByIntervierIdQuery, List<HiringStage>>
{
    private readonly IHiringStageRepository stages;

    public GetHiringStageByIntervierIdHandler(IHiringStageRepository repository)
    {
        stages = repository;
    }

    public async Task<List<HiringStage>> Handle(GetHiringStageByIntervierIdQuery request, CancellationToken cancellationToken)
    {
        return await stages.GetByIntervierIdAsync(request.IntervierId);
    }
}