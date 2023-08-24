using MediatR;
using ProjectManagementService.Application.Abstractions;
using ProjectManagementService.Application.Exceptions.Project;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectQueries;

public class GetCustomerProjectByIdHandler : IRequestHandler<GetCustomerProjectByIdQuery, Project>
{
    private readonly IProjectRepository _projectRepository;

    public GetCustomerProjectByIdHandler(IProjectRepository repository)
    {
        _projectRepository = repository;
    }

    public async Task<Project> Handle(GetCustomerProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId);

        if (project is null) throw new NoProjectWithSuchIdException();

        if (project.CustomerId != request.CustomerId) throw new CustomerAccessToProjecDeniedException();

        return project;
    }
}
