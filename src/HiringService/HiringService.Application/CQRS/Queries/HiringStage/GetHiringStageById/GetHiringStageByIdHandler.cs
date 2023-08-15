using HiringService.Application.Abstractions;
using HiringService.Application.Exceptions.HiringStage;
using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.HiringStageQueries;

public class GetHiringStageByIdHandler : IRequestHandler<GetHiringStageByIdQuery, HiringStage>
{
    private readonly IHiringStageRepository _stageRepository;

    public GetHiringStageByIdHandler(IHiringStageRepository stageRepository)
    {
        _stageRepository = stageRepository;
    }

    public async Task<HiringStage> Handle(GetHiringStageByIdQuery request, CancellationToken cancellationToken)
    {
        var hiringStage = await _stageRepository.GetByIdAsync(request.Id);

        if (hiringStage is null) throw new NoHiringStageWithSuchIdException();

        return hiringStage;
    }
}
