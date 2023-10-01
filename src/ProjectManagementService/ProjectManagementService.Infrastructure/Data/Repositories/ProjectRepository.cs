using ProjectManagementService.Domain.Entities;
using MongoDB.Driver;
using HiringService.Infrastructure.Data.Repositories;
using ProjectManagementService.Application.Configuration;
using Microsoft.Extensions.Options;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Domain.Enumerations;

namespace ProjectManagementService.Infrastucture.Data.Repositories;

public class ProjectRepository : GenericRepository<Project>, IProjectRepository
{
    public ProjectRepository(IMongoClient mongoClient,
        IOptions<MongoDBConfiguration> DBconfiguration) :
        base(mongoClient, DBconfiguration)
    { }

    public async Task<List<Project>> GetAllCustomerProjectsAsync(string customerId) 
    {
        return await GetFilteredAsync(project => project.CustomerId == customerId);
    }

    public async Task<Project?> GetProjectByProjectLeaderId(string projectLeaderId) 
    {
        return await GetFirstAsync(task => task.LeadWorkerId == projectLeaderId);
    }

    public async Task CancelAsync(string id)
    {
        await UpdatePropertyAsync(id, "status", ProjectStatus.Canceled);
    }

    public async Task UpdateObjectiveAsync(string id, string objective)
    {
        await UpdatePropertyAsync(id, "objective", objective);
    }

    public async Task UpdateDescriptionAsync(string id, string description)
    {
        await UpdatePropertyAsync(id, "description", description);
    }
}
