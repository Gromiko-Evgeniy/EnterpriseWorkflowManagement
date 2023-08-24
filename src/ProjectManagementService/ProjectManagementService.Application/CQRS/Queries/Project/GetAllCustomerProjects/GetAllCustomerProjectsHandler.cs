using MediatR;
using ProjectManagementService.Application.Abstractions;
using ProjectManagementService.Application.Exceptions.Customer;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectQueries;

public class GetAllCustomerProjectsHandler : IRequestHandler<GetAllCustomerProjectsQuery, List<Project>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ICustomerRepository _customerRepository;

    public GetAllCustomerProjectsHandler(IProjectRepository projectRepository, ICustomerRepository customerRepository)
    {
        _projectRepository = projectRepository;
        _customerRepository = customerRepository;
    }

    public async Task<List<Project>> Handle(GetAllCustomerProjectsQuery request, CancellationToken cancellationToken)
    {
        var customer = _customerRepository.GetByIdAsync(request.CustomerId);

        if (customer is null) throw new NoCandidateWithSuchIdException();

        return await _projectRepository.GetAllCustomerProjectsAsync(request.CustomerId);
    }
}
