using MediatR;
using ProjectManagementService.Application.Abstractions;
using ProjectManagementService.Application.Exceptions.Project;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectTaskQueries; 
public class GetProjectTasksByProjectIdHandler : IRequestHandler<GetProjectTasksByProjectIdQuery, List<ProjectTask>>
{
    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly IProjectRepository _projectRepository;

    public GetProjectTasksByProjectIdHandler(IProjectTaskRepository projectTaskRepository, IProjectRepository projectRepository)
    {
        _projectTaskRepository = projectTaskRepository;
        _projectRepository = projectRepository;
    }

    public async Task<List<ProjectTask>> Handle(GetProjectTasksByProjectIdQuery request, CancellationToken cancellationToken)
    {
        var project = _projectRepository.GetByIdAsync(request.ProjectId);

        if (project is null) throw new NoProjectWithSuchIdException();

        var projects = await _projectTaskRepository.GetByProjectIdAsync(request.ProjectId);

        return projects;
    }
}