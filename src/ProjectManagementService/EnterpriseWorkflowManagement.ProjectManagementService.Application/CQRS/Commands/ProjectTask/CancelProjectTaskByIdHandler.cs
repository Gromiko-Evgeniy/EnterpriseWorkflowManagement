using MediatR;
using ProjectManagementService.Application.Abstractions;

namespace ProjectManagementService.Application.CQRS.ProjectTaskCommands;

public class CancelProjectTaskByIdHandler : IRequestHandler<CancelProjectTaskByIdCommand>
{
    private readonly IProjectTasksRepository tasksRepository;
    private readonly IProjectsRepository projectsRepository;

    public CancelProjectTaskByIdHandler(IProjectTasksRepository tasksRepository, IProjectsRepository projectsRepository)
    {
        this.tasksRepository = tasksRepository;
        this.projectsRepository = projectsRepository;
    }

    async Task<Unit> IRequestHandler<CancelProjectTaskByIdCommand, Unit>.Handle(CancelProjectTaskByIdCommand request, CancellationToken cancellationToken)
    {
        var task = await tasksRepository.GetByIdAsync(request.ProjectTaskId);
        var customerProjects = await projectsRepository.GetAllCustomerProjectsAsync(request.CustomerId);

        if (!customerProjects.Any(p => p.Id == task.ProjectId)) return Unit.Value; // throw ex

        await tasksRepository.CancelAsync(request.ProjectTaskId);

        return Unit.Value; //fake empty value
    }
}