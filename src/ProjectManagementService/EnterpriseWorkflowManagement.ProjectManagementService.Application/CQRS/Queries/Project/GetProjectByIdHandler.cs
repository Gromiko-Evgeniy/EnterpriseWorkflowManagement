using MediatR;
using ProjectManagementService.Application.Abstractions;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectQueries;
public class GetProjectByIdHandler : IRequestHandler<GetProjectByIdQuery, Project>
{
    private readonly IProjectsRepository projectRepository;

    public GetProjectByIdHandler(IProjectsRepository repository)
    {
        projectRepository = repository;
    }

    public async Task<Project> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        return await projectRepository.GetByIdAsync(request.Id);
    }
}