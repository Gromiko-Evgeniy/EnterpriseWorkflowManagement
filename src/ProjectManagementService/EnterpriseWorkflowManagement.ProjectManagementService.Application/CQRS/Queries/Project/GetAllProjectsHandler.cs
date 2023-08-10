using ProjectManagementService.Domain.Entities;
using ProjectManagementService.Application.Abstractions;
using MediatR;

namespace ProjectManagementService.Application.CQRS.ProjectQueries;
public class GetAllProjectsHandler : IRequestHandler<GetAllProjectsQuery, List<Project>>
{
    private readonly IProjectsRepository projectRepository;

    public GetAllProjectsHandler(IProjectsRepository repository)
    {
        projectRepository = repository;
    }

    public async Task<List<Project>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        return await projectRepository.GetAllAsync();
    }
}
