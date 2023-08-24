using MediatR;
using ProjectManagementService.Application.Abstractions;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectTaskQueries;

public class GetAllProjectTasksHandler : IRequestHandler<GetAllProjectTasksQuery, List<ProjectTask>>
{
    private readonly IProjectTaskRepository _projectTaskRepository;

    public GetAllProjectTasksHandler(IProjectTaskRepository repository)
    {
        _projectTaskRepository = repository;
    }

    public async Task<List<ProjectTask>> Handle(GetAllProjectTasksQuery request, CancellationToken cancellationToken)
    {
        return await _projectTaskRepository.GetAllAsync();
    }
}
