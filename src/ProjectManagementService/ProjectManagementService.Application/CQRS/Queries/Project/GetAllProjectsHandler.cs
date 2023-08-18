using ProjectManagementService.Domain.Entities;
using ProjectManagementService.Application.Abstractions;
using MediatR;

namespace ProjectManagementService.Application.CQRS.ProjectQueries;
public class GetAllProjectsHandler : IRequestHandler<GetAllProjectsQuery, List<Project>>
{
    private readonly IProjectRepository _projectRepository;

    public GetAllProjectsHandler(IProjectRepository repository)
    {
        _projectRepository = repository;
    }

    public async Task<List<Project>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        return await _projectRepository.GetAllAsync();
    }
}
