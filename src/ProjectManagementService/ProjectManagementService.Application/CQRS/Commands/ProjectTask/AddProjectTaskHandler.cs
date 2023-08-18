using AutoMapper;
using MediatR;
using ProjectManagementService.Application.Abstractions;
using ProjectManagementService.Application.Exceptions.Project;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectTaskCommands;

public class AddProjectTaskHandler : IRequestHandler<AddProjectTaskCommand, string>
{
    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public AddProjectTaskHandler(IProjectTaskRepository projectTaskRepository,
        IProjectRepository projectRepository, IMapper mapper)
    {
        _projectTaskRepository = projectTaskRepository;
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    public async Task<string> Handle(AddProjectTaskCommand request, CancellationToken cancellationToken)
    {
        var projectTaskDTO = request.ProjectTaskDTO;

        var project = _projectRepository.GetByIdAsync(projectTaskDTO.ProjectId);

        if (project is null) throw new NoProjectWithSuchIdException();

        var newProjectTask = _mapper.Map<ProjectTask>(projectTaskDTO);
          
        return await _projectTaskRepository.AddAsync(newProjectTask);
    }
}
