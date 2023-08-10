using MediatR;
using ProjectManagementService.Application.Abstractions;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectTaskQueries;

public class GetProjectTaskByIdHandler : IRequestHandler<GetProjectTaskByIdQuery, ProjectTask>
{
    private readonly IProjectTasksRepository projectTasksRepository;

    public GetProjectTaskByIdHandler(IProjectTasksRepository repository)
    {
        projectTasksRepository = repository;
    }

    public async Task<ProjectTask> Handle(GetProjectTaskByIdQuery request, CancellationToken cancellationToken)
    {
        return await projectTasksRepository.GetByIdAsync(request.Id);
    }
}