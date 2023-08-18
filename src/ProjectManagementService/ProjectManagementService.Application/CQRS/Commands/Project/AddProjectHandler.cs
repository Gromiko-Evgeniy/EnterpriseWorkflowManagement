using AutoMapper;
using MediatR;
using ProjectManagementService.Application.Abstractions;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectCommands;
public class AddProjectHandler : IRequestHandler<AddProjectCommand, string>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public AddProjectHandler(IProjectRepository repository, IMapper mapper)
    {
        _mapper = mapper;
        _projectRepository = repository;
    }

    public async Task<string> Handle(AddProjectCommand request, CancellationToken cancellationToken)
    {
        var projectDTO = request.ProjectDTO;

        var newProject = _mapper.Map<Project>(projectDTO);

        string id = await _projectRepository.AddAsync(newProject);

        return id;
    }
}