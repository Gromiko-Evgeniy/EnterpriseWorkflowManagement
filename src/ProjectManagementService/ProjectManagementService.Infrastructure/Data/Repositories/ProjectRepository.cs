using ProjectManagementService.Domain.Entities;
using MongoDB.Driver;
using ProjectManagementService.Application.Abstractions;
using HiringService.Infrastructure.Data.Repositories;
using ProjectManagementService.Application.Configuration;
using Microsoft.Extensions.Options;

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
}
