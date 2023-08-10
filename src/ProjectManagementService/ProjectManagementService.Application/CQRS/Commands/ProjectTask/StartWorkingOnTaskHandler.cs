using MediatR;
using ProjectManagementService.Application.Abstractions;

namespace ProjectManagementService.Application.CQRS.ProjectTaskCommands;

public class StartWorkingOnTaskHandler : IRequestHandler<StartWorkingOnTaskCommand>
{
    private readonly IWorkersRepository workersRepository;
    private readonly IProjectTasksRepository tasksRepository;

    public StartWorkingOnTaskHandler(IWorkersRepository workersRepository, IProjectTasksRepository tasksRepository)
    {
        this.workersRepository = workersRepository;
        this.tasksRepository = tasksRepository;
    }

    async Task<Unit> IRequestHandler<StartWorkingOnTaskCommand, Unit>.Handle(StartWorkingOnTaskCommand request, CancellationToken cancellationToken)
    {
        var worker = await workersRepository.GetByIdAsync(request.WorkerId);

        await tasksRepository.StartWorkingOnTask(worker.CurrentTaskId);

        return Unit.Value; //fake empty value
    }
}