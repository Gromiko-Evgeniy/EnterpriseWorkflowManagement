using HiringService.Application.Abstractions;
using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.HiringStageQueries;

public class GetHiringStagesHandler : IRequestHandler<GetHiringStagesQuery, List<HiringStage>>
{
    private readonly IHiringStageRepository _stageRepository;

    public GetHiringStagesHandler(IHiringStageRepository stageRepository)
    {
        _stageRepository = stageRepository;
    }

    public async Task<List<HiringStage>> Handle(GetHiringStagesQuery request, CancellationToken cancellationToken)
    {
        return await _stageRepository.GetAllAsync();
    }
}

