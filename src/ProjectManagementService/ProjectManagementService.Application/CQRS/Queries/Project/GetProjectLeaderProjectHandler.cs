using MediatR;
using ProjectManagementService.Application.Abstractions;
using ProjectManagementService.Application.Exceptions.Worker;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectQueries;

public class GetProjectLeaderProjectHandler : IRequestHandler<GetProjectLeaderProjectQuery, Project>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IWorkerRepository _workerRepository;


    public GetProjectLeaderProjectHandler(IProjectRepository repository, IWorkerRepository workerRepository)
    {
        _projectRepository = repository;
        _workerRepository = workerRepository;
    }

    public async Task<Project> Handle(GetProjectLeaderProjectQuery request, CancellationToken cancellationToken)
    {
        var projectLeader = await _workerRepository.GetByIdAsync(request.ProjectLeaderId);

        if (projectLeader is null) throw new NoWorkerWithSuchIdException();

        return await _projectRepository.GetProjectLeaderProject(request.ProjectLeaderId);
    }
}