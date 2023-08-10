using MediatR;
using ProjectManagementService.Application.Abstractions;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectTaskQueries; 
public class GetProjectTasksByProjectIdHandler : IRequestHandler<GetProjectTasksByProjectIdQuery, List<ProjectTask>>
{
    private readonly IProjectTasksRepository projectTasksRepository;

    public GetProjectTasksByProjectIdHandler(IProjectTasksRepository repository)
    {
        projectTasksRepository = repository;
    }

    public async Task<List<ProjectTask>> Handle(GetProjectTasksByProjectIdQuery request, CancellationToken cancellationToken)
    {
        var projects = await projectTasksRepository.GetByProjectIdAsync(request.ProjectId);

        return projects;
    }
}