using MediatR;
using ProjectManagementService.Application.Abstractions;
using ProjectManagementService.Application.Exceptions.Worker;

namespace ProjectManagementService.Application.CQRS.ProjectTaskCommands;

public class MarkProjectTaskAsReadyToApproveHandler : IRequestHandler<MarkProjectTaskAsReadyToApproveCommand>
{
    private readonly IWorkerRepository _workerRepository;
    private readonly IProjectTaskRepository _taskRepository;

    public MarkProjectTaskAsReadyToApproveHandler(IWorkerRepository workersRepository, IProjectTaskRepository taskRepository)
    {
        _workerRepository = workersRepository;
        _taskRepository = taskRepository;
    }

    async Task<Unit> IRequestHandler<MarkProjectTaskAsReadyToApproveCommand, Unit>.Handle(MarkProjectTaskAsReadyToApproveCommand request, CancellationToken cancellationToken)
    {
        var worker = await _workerRepository.GetByIdAsync(request.WorkerId);

        if (worker is null) throw new NoWorkerWithSuchIdException();
        if (worker.CurrentTaskId is null) throw new WorkerHasNoTaskNowException();

        await _taskRepository.MarkAsReadyToApproveAsync(worker.CurrentTaskId);

        return Unit.Value; //fake empty value
    }
}
