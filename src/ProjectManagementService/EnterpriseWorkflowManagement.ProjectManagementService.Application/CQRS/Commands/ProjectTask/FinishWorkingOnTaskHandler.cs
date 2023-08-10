using MediatR;
using ProjectManagementService.Application.Abstractions;

namespace ProjectManagementService.Application.CQRS.ProjectTaskCommands;

public class FinishWorkingOnTaskHandler : IRequestHandler<FinishWorkingOnTaskCommand>
{
    private readonly IWorkersRepository workersRepository;
    private readonly IProjectTasksRepository tasksRepository;

    public FinishWorkingOnTaskHandler(IWorkersRepository workersRepository, IProjectTasksRepository tasksRepository)
    {
        this.workersRepository = workersRepository;
        this.tasksRepository = tasksRepository;
    }

    async Task<Unit> IRequestHandler<FinishWorkingOnTaskCommand, Unit>.Handle(FinishWorkingOnTaskCommand request, CancellationToken cancellationToken)
    {
        //check if task was starded
        
        var worker = await workersRepository.GetByIdAsync(request.WorkerId);

        await tasksRepository.FinishWorkingOnTask(worker.CurrentTaskId);

        return Unit.Value; //fake empty value
    }
}