using MediatR;
using ProjectManagementService.Application.Abstractions;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectCommands;
public class AddProjectHandler : IRequestHandler<AddProjectCommand, string>
{
    private readonly IProjectsRepository projectRepository;

    public AddProjectHandler(IProjectsRepository repository)
    {
        projectRepository = repository;
    }

    public async Task<string> Handle(AddProjectCommand request, CancellationToken cancellationToken)
    {
        var projectDTO = request.ProjectDTO;

        var newProject = new Project() // use automapper
        { 
            Objective = projectDTO.Objective,
            Description = projectDTO.Description,
            CustomerId = request.CustomerId
        };

        string id = await projectRepository.AddAsync(newProject);

        return id;
    }
}