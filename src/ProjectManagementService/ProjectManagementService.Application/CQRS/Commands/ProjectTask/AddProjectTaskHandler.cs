using MediatR;
using ProjectManagementService.Application.Abstractions;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectTaskCommands;

public class AddProjectTaskHandler : IRequestHandler<AddProjectTaskCommand, string>
{
    private readonly IProjectTasksRepository projectTasksRepository;

    public AddProjectTaskHandler(IProjectTasksRepository repository)
    {
        projectTasksRepository = repository;
    }

    public async Task<string> Handle(AddProjectTaskCommand request, CancellationToken cancellationToken)
    {
        var projectTaskDTO = request.ProjectTaskDTO;

        var newProjectTask = new ProjectTask() // use automapper
        {
            Name = projectTaskDTO.Name,
            Description = projectTaskDTO.Description,
            Priority = projectTaskDTO.Priority,
            ProjectId = projectTaskDTO.ProjectId
        };

        return await projectTasksRepository.AddAsync(newProjectTask);
    }
}
