using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.Abstractions;
public interface IProjectTasksRepository
{
    public Task<List<ProjectTask>> GetAllAsync();

    public Task<ProjectTask> GetByIdAsync(string id);

    public Task<List<ProjectTask>> GetByProjectIdAsync(string projectId);

    public Task<string> AddAsync(ProjectTask task); // AddProjectTaskDTO task + mapping = service?  

    public Task CancelAsync(string id);

    public Task MarkAsReadyToApproveAsync(string id);

    public Task MarkAsApproved(string id);

    public Task StartWorkingOnTask(string id);

    public Task FinishWorkingOnTask(string id);
}
