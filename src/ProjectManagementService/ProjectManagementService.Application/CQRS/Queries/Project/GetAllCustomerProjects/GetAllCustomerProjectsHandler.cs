using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Routing.Matching;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.ProjectDTOs;
using ProjectManagementService.Application.Exceptions.Customer;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectQueries;

public class GetAllCustomerProjectsHandler : IRequestHandler<GetAllCustomerProjectsQuery, List<ProjectShortInfoDTO>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public GetAllCustomerProjectsHandler(
        IProjectRepository projectRepository,
        ICustomerRepository customerRepository,
        IMapper mapper)
    {
        _projectRepository = projectRepository;
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<List<ProjectShortInfoDTO>> Handle(GetAllCustomerProjectsQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId);

        if (customer is null) throw new NoCustomerWithSuchIdException();

        var projects = await _projectRepository.GetAllCustomerProjectsAsync(request.CustomerId);

        return projects.Select(_mapper.Map<ProjectShortInfoDTO>).ToList();
    }
}
