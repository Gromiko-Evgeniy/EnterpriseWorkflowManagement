using MediatR;
using ProjectManagementService.Application.Abstractions;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectQueries;
public class GetAllCustomerProjectsHandler : IRequestHandler<GetAllCustomerProjectsQuery, List<Project>>
{
    private readonly IProjectsRepository projectRepository;

    public GetAllCustomerProjectsHandler(IProjectsRepository repository)
    {
        projectRepository = repository;
    }

    public async Task<List<Project>> Handle(GetAllCustomerProjectsQuery request, CancellationToken cancellationToken)
    {
        return await projectRepository.GetAllCustomerProjectsAsync(request.CustomerId);
    }
}