using MediatR;
using ProjectManagementService.Application.Abstractions;
using ProjectManagementService.Application.Exceptions.Worker;

namespace ProjectManagementService.Application.CQRS.ProjectTaskCommands;

public class StartWorkingOnTaskHandler : IRequestHandler<StartWorkingOnTaskCommand>
{
    private readonly IWorkerRepository _workerRepository;
    private readonly IProjectTaskRepository _taskRepository;

    public StartWorkingOnTaskHandler(IWorkerRepository workersRepository, IProjectTaskRepository taskRepository)
    {
        _workerRepository = workersRepository;
        _taskRepository = taskRepository;
    }

    async Task<Unit> IRequestHandler<StartWorkingOnTaskCommand, Unit>.Handle(StartWorkingOnTaskCommand request, CancellationToken cancellationToken)
    {
        var worker = await _workerRepository.GetByIdAsync(request.WorkerId);

        if (worker is null) throw new NoWorkerWithSuchIdException();

        if (worker.CurrentTaskId is null) throw new WorkerHasNoTaskNowException();

        await _taskRepository.StartWorkingOnTask(worker.CurrentTaskId);

        return Unit.Value; //fake empty value
    }
}