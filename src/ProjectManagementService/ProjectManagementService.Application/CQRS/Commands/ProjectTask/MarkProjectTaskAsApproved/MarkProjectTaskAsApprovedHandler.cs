using MediatR;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.Exceptions.Project;
using ProjectManagementService.Application.Exceptions.ProjectTask;

namespace ProjectManagementService.Application.CQRS.ProjectTaskCommands;

public class MarkProjectTaskAsApprovedHandler : IRequestHandler<MarkProjectTaskAsApprovedCommand>
{
    private readonly IProjectTaskRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;

    public MarkProjectTaskAsApprovedHandler(IProjectTaskRepository tasksRepository, IProjectRepository projectRepository)
    {
        _taskRepository = tasksRepository;
        _projectRepository = projectRepository;
    }

    public async Task<Unit> Handle(MarkProjectTaskAsApprovedCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.ProjectTaskId);

        if (task is null) throw new NoProjectTaskWithSuchIdException();

        var projectLeaderProject = await _projectRepository.GetProjectByProjectLeaderId(request.ProjectLeaderId);

        if (projectLeaderProject is null) throw new NoProjectWithSuchIdException();
        if (task.ProjectId != projectLeaderProject.Id) throw new AccessToApproveProjectTaskDeniedException();

        await _taskRepository.MarkAsApproved(request.ProjectTaskId);

        return Unit.Value; //fake empty value
    }
}
