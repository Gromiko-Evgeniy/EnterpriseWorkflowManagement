using HiringService.Application.Abstractions;
using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.StageQueries;

public class GetHiringStageByIdHandler : IRequestHandler<GetHiringStageByIdQuery, HiringStage>
{
    private readonly IHiringStageRepository stages;

    public GetHiringStageByIdHandler(IHiringStageRepository repository)
    {
        stages = repository;
    }

    public async Task<HiringStage> Handle(GetHiringStageByIdQuery request, CancellationToken cancellationToken)
    {
        return await stages.GetByIdAsync(request.Id);
    }
}
