using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.Abstractions;
public interface IProjectsRepository
{
    public Task<List<Project>> GetAllAsync();

    public Task<Project> GetByIdAsync(string id);

    public Task<List<Project>> GetAllCustomerProjectsAsync(string customerId);

    public Task<Project> GetProjectLeaderProject(string projectLeaderId);

    public Task<string> AddAsync(Project candidate);
}
