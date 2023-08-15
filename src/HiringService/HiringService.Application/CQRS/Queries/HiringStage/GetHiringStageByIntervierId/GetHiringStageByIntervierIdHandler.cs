using HiringService.Application.Abstractions;
using HiringService.Application.Exceptions.Candidate;
using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.HiringStageQueries;

public class GetHiringStageByIntervierIdHandler : IRequestHandler<GetHiringStageByIntervierIdQuery, List<HiringStage>>
{
    private readonly IHiringStageRepository _stageRepository;
    private readonly IWorkerRepository _workerRepository;

    public GetHiringStageByIntervierIdHandler(IHiringStageRepository stageRepository, IWorkerRepository workerRepository)
    {
        _stageRepository = stageRepository;
        _workerRepository = workerRepository;
    }

    public async Task<List<HiringStage>> Handle(GetHiringStageByIntervierIdQuery request, CancellationToken cancellationToken)
    {
        var worker = await _workerRepository.GetByIdAsync(request.IntervierId);

        if (worker is null) throw new NoWorkerWithSuchIdException();

        return await _stageRepository.GetByIntervierIdAsync(request.IntervierId);
    }
}