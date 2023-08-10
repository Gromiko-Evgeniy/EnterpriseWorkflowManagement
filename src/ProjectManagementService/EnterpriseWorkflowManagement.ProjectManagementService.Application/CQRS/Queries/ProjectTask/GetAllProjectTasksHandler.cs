using MediatR;
using ProjectManagementService.Application.Abstractions;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectTaskQueries;

public class GetAllProjectTasksHandler : IRequestHandler<GetAllProjectTasksQuery, List<ProjectTask>>
{
    private readonly IProjectTasksRepository projectTasksRepository;

    public GetAllProjectTasksHandler(IProjectTasksRepository repository)
    {
        projectTasksRepository = repository;
    }

    public async Task<List<ProjectTask>> Handle(GetAllProjectTasksQuery request, CancellationToken cancellationToken)
    {
        return await projectTasksRepository.GetAllAsync();
    }
}