using MediatR;
using ProjectManagementService.Application.Abstractions;
using ProjectManagementService.Application.Exceptions.Project;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectTaskQueries;

public class GetProjectTaskByIdHandler : IRequestHandler<GetProjectTaskByIdQuery, ProjectTask>
{
    private readonly IProjectTaskRepository _projectTaskRepository;

    public GetProjectTaskByIdHandler(IProjectTaskRepository repository)
    {
        _projectTaskRepository = repository;
    }

    public async Task<ProjectTask> Handle(GetProjectTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var task = await _projectTaskRepository.GetByIdAsync(request.Id);

        if (task == null) throw new NoProjectWithSuchIdException();

        return task;
    }
}