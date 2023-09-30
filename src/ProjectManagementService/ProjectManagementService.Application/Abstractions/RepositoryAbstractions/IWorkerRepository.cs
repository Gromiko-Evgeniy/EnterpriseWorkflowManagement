using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.Abstractions.RepositoryAbstractions;

public interface IWorkerRepository : IGenericRepository<Worker>
{
    public Task UpdateTaskAsync(string workerId, string taskId);

    public Task UpdateProjectAsync(string workerId, string projectId);
}
