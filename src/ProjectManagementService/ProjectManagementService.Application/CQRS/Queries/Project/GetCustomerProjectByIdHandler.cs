using MediatR;
using ProjectManagementService.Application.Abstractions;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectQueries;
public class GetCustomerProjectByIdHandler : IRequestHandler<GetCustomerProjectByIdQuery, Project>
{
    private readonly IProjectsRepository projectRepository;

    public GetCustomerProjectByIdHandler(IProjectsRepository repository)
    {
        projectRepository = repository;
    }

    public async Task<Project> Handle(GetCustomerProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetByIdAsync(request.ProjectId);

        if (project is null) return null;

        if (project.CustomerId != request.CustomerId) return null;

        return project;
    }
}