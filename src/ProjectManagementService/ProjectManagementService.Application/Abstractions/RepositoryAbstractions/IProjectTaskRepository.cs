using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.Abstractions.RepositoryAbstractions;

public interface IProjectTaskRepository : IGenericRepository<ProjectTask>
{
    public new Task<string> AddAsync(ProjectTask project);

    public Task<List<ProjectTask>> GetByProjectIdAsync(string projectId);

    public Task CancelAsync(string id);

    public Task MarkAsReadyToApproveAsync(string id);

    public Task MarkAsApproved(string id);

    public Task StartWorkingOnTask(string id);

    public Task FinishWorkingOnTask(string id);
}
