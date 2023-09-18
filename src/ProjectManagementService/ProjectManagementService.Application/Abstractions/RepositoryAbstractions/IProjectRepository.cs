using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.Abstractions.RepositoryAbstractions;

public interface IProjectRepository : IGenericRepository<Project>
{
    public Task<List<Project>> GetAllCustomerProjectsAsync(string customerId);

    public Task<Project?> GetProjectByProjectLeaderId(string projectLeaderId);
    
    public Task CancelAsync(string id);
}
