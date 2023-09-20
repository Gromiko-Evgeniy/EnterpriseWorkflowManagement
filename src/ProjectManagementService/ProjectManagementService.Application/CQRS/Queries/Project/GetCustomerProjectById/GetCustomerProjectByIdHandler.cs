using AutoMapper;
using MediatR;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.DTOs.ProjectDTOs;
using ProjectManagementService.Application.Exceptions.Project;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectQueries;

public class GetCustomerProjectByIdHandler : IRequestHandler<GetCustomerProjectByIdQuery, ProjectMainInfoDTO>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public GetCustomerProjectByIdHandler(IProjectRepository repository, IMapper mapper)
    {
        _projectRepository = repository;
        _mapper = mapper;
    }

    public async Task<ProjectMainInfoDTO> Handle(GetCustomerProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId);

        if (project is null) throw new NoProjectWithSuchIdException();

        if (project.CustomerId != request.CustomerId) throw new CustomerAccessToProjectDeniedException();

        return _mapper.Map<ProjectMainInfoDTO>(project);
    }
}
