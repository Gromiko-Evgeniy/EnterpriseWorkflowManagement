using MediatR;
using ProjectManagementService.Application.Abstractions;
using ProjectManagementService.Application.Exceptions.Worker;

namespace ProjectManagementService.Application.CQRS.ProjectTaskCommands;

public class FinishWorkingOnTaskHandler : IRequestHandler<FinishWorkingOnTaskCommand>
{
    private readonly IWorkerRepository _workerRepository;
    private readonly IProjectTaskRepository _taskRepository;

    public FinishWorkingOnTaskHandler(IWorkerRepository workersRepository, IProjectTaskRepository taskRepository)
    {
        _workerRepository = workersRepository;
        _taskRepository = taskRepository;
    }

    public async Task<Unit> Handle(FinishWorkingOnTaskCommand request, CancellationToken cancellationToken)
    {
        var worker = await _workerRepository.GetByIdAsync(request.WorkerId);

        if (worker is null) throw new NoWorkerWithSuchIdException();

        if (worker.CurrentTaskId is null) throw new WorkerHasNoTaskNowException();

        await _taskRepository.FinishWorkingOnTask(worker.CurrentTaskId);

        return Unit.Value; //fake empty value
    }
}
