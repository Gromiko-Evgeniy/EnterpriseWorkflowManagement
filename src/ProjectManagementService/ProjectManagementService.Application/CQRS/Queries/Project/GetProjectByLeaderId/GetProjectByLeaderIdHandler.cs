using MediatR;
using ProjectManagementService.Application.Abstractions;
using ProjectManagementService.Application.Exceptions.Project;
using ProjectManagementService.Application.Exceptions.Worker;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectQueries;

public class GetProjectByLeaderIdHandler : IRequestHandler<GetProjectByLeaderIdQuery, Project>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IWorkerRepository _workerRepository;


    public GetProjectByLeaderIdHandler(IProjectRepository repository, IWorkerRepository workerRepository)
    {
        _projectRepository = repository;
        _workerRepository = workerRepository;
    }

    public async Task<Project> Handle(GetProjectByLeaderIdQuery request, CancellationToken cancellationToken)
    {
        var projectLeader = await _workerRepository.GetByIdAsync(request.ProjectLeaderId);

        if (projectLeader is null) throw new NoWorkerWithSuchIdException();

        var project = await _projectRepository.GetProjectByProjectLeaderId(request.ProjectLeaderId);

        if (project is null) throw new NoProjectWithSuchProjectLeader();

        return project;
    }
}
