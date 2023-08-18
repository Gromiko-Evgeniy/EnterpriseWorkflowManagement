using HiringService.Application.Abstractions;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.Abstractions;

public interface IProjectRepository : IGenericRepository<Project>
{
    public Task<List<Project>> GetAllCustomerProjectsAsync(string customerId);

    public Task<Project> GetProjectLeaderProject(string projectLeaderId);

    public Task<string> AddAsync(Project project);

}
