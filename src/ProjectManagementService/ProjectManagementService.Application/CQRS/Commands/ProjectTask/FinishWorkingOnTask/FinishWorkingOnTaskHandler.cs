using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.Exceptions.Worker;

namespace ProjectManagementService.Application.CQRS.ProjectTaskCommands;

public class FinishWorkingOnTaskHandler : IRequestHandler<FinishWorkingOnTaskCommand>
{
    private readonly IWorkerRepository _workerRepository;
    private readonly IProjectTaskRepository _taskRepository;
    private readonly IDistributedCache _cache;

    public FinishWorkingOnTaskHandler(IWorkerRepository workersRepository,
        IProjectTaskRepository taskRepository, IDistributedCache cache)
    {
        _workerRepository = workersRepository;
        _taskRepository = taskRepository;
        _cache = cache;
    }

    public async Task<Unit> Handle(FinishWorkingOnTaskCommand request, CancellationToken cancellationToken)
    {
        var worker = await _workerRepository.GetByIdAsync(request.WorkerId);

        if (worker is null) throw new NoWorkerWithSuchIdException();

        if (worker.CurrentTaskId is null) throw new WorkerHasNoTaskNowException();

        await _taskRepository.FinishWorkingOnTask(worker.CurrentTaskId);

        var idKey = "Task_" + worker.CurrentTaskId;
        await _cache.RemoveAsync(idKey);

        return Unit.Value; //fake empty value
    }
}
