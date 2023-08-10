using MediatR;
using ProjectManagementService.Application.Abstractions;

namespace ProjectManagementService.Application.CQRS.ProjectTaskCommands;

public class MarkProjectTaskAsApprovedHandler : IRequestHandler<MarkProjectTaskAsApprovedCommand>
{
    private readonly IProjectTasksRepository tasksRepository;
    private readonly IProjectsRepository projectsRepository;

    public MarkProjectTaskAsApprovedHandler(IProjectTasksRepository tasksRepository, IProjectsRepository projectsRepository)
    {
        this.tasksRepository = tasksRepository;
        this.projectsRepository = projectsRepository;
    }

    async Task<Unit> IRequestHandler<MarkProjectTaskAsApprovedCommand, Unit>.Handle(MarkProjectTaskAsApprovedCommand request, CancellationToken cancellationToken)
    {
        var task = await tasksRepository.GetByIdAsync(request.ProjectTaskId);

        var projectLeaderProject = await projectsRepository.GetProjectLeaderProject(request.ProjectLeaderId);

        if (task.ProjectId != projectLeaderProject.Id) return Unit.Value; // throw ex "ProjectLeader can only approve tasks of his project"

        await tasksRepository.MarkAsApproved(request.ProjectTaskId);

        return Unit.Value; //fake empty value
    }
}