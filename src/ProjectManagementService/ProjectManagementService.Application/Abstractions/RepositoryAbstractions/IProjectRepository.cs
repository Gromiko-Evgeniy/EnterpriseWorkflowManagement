using MongoDB.Bson;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.Abstractions.RepositoryAbstractions;

public interface IProjectRepository : IGenericRepository<Project>
{
    public Task<List<Project>> GetAllCustomerProjectsAsync(string customerId);

    public Task<Project?> GetProjectByProjectLeaderId(string projectLeaderId);
    
    public Task CancelAsync(string id);

    public Task UpdateObjectiveAsync(string id, string objective);

    public Task UpdateDescriptionAsync(string id, string description);
}
