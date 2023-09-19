using HiringService.Application.Cache;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.Exceptions.Worker;

namespace ProjectManagementService.Application.CQRS.ProjectTaskCommands;

public class MarkProjectTaskAsReadyToApproveHandler : IRequestHandler<MarkProjectTaskAsReadyToApproveCommand>
{
    private readonly IWorkerRepository _workerRepository;
    private readonly IProjectTaskRepository _taskRepository;
    private readonly IDistributedCache _cache;

    public MarkProjectTaskAsReadyToApproveHandler(IWorkerRepository workersRepository,
        IProjectTaskRepository taskRepository, IDistributedCache cache)
    {
        _workerRepository = workersRepository;
        _taskRepository = taskRepository;
        _cache = cache;
    }

    public async Task<Unit> Handle(MarkProjectTaskAsReadyToApproveCommand request, CancellationToken cancellationToken)
    {
        var worker = await _workerRepository.GetByIdAsync(request.WorkerId);

        if (worker is null) throw new NoWorkerWithSuchIdException();
        if (worker.CurrentTaskId is null) throw new WorkerHasNoTaskNowException();

        await _taskRepository.MarkAsReadyToApproveAsync(worker.CurrentTaskId);

        var idKey = RedisKeysPrefixes.ProjectTaskPrefix + worker.CurrentTaskId;
        await _cache.RemoveAsync(idKey);

        return Unit.Value; //fake empty value
    }
}
