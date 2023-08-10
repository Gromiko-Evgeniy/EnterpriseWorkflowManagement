using MediatR;
using ProjectManagementService.Application.Abstractions;

namespace ProjectManagementService.Application.CQRS.ProjectTaskCommands;

public class MarkProjectTaskAsReadyToApproveHandler : IRequestHandler<MarkProjectTaskAsReadyToApproveCommand>
{
    private readonly IWorkersRepository workersRepository;
    private readonly IProjectTasksRepository tasksRepository;

    public MarkProjectTaskAsReadyToApproveHandler(IWorkersRepository workersRepository, IProjectTasksRepository tasksRepository)
    {
        this.workersRepository = workersRepository;
        this.tasksRepository = tasksRepository;
    }

    async Task<Unit> IRequestHandler<MarkProjectTaskAsReadyToApproveCommand, Unit>.Handle(MarkProjectTaskAsReadyToApproveCommand request, CancellationToken cancellationToken)
    {
        var woeker = await workersRepository.GetByIdAsync(request.WorkerId);

        if (woeker.CurrentTaskId != request.ProjectTaskId) return Unit.Value; // throw ex "WorkerId can mark ReadyToApprove only his own task"

        await tasksRepository.MarkAsReadyToApproveAsync(request.ProjectTaskId);

        return Unit.Value; //fake empty value
    }
}