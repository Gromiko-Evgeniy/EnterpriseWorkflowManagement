using MediatR;
using ProjectManagementService.Application.Abstractions;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectQueries;

public class GetProjectLeaderProjectHandler : IRequestHandler<GetProjectLeaderProjectQuery, Project>
{
    private readonly IProjectsRepository projectRepository;

    public GetProjectLeaderProjectHandler(IProjectsRepository repository)
    {
        projectRepository = repository;
    }

    public async Task<Project> Handle(GetProjectLeaderProjectQuery request, CancellationToken cancellationToken)
    {
        return await projectRepository.GetProjectLeaderProject(request.ProjectLeaderId);
    }
}