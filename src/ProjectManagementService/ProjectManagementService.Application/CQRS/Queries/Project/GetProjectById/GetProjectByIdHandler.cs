using AutoMapper;
using MediatR;
using ProjectManagementService.Application.Abstractions;
using ProjectManagementService.Application.DTOs.ProjectDTOs;
using ProjectManagementService.Application.Exceptions.Project;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectQueries;

public class GetProjectByIdHandler : IRequestHandler<GetProjectByIdQuery, ProjectMainInfoDTO>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public GetProjectByIdHandler(IProjectRepository repository, IMapper mapper)
    {
        _projectRepository = repository;
        _mapper = mapper;
    }

    public async Task<ProjectMainInfoDTO> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.Id);

        if (project is null) throw new NoProjectWithSuchIdException();

        return _mapper.Map<ProjectMainInfoDTO>(project);
    }
}
