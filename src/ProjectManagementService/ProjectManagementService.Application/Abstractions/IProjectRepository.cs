using HiringService.Application.Abstractions;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.Abstractions;

public interface IProjectRepository : IGenericRepository<Project>
{
    public Task<List<Project>> GetAllCustomerProjectsAsync(string customerId);

    public Task<Project?> GetProjectByProjectLeaderId(string projectLeaderId);

    public new Task<string> AddAsync(Project project);
}
