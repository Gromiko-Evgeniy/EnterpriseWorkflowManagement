using ProjectManagementService.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ProjectManagementService.Application.Abstractions;
using HiringService.Infrastructure.Data.Repositories;

namespace ProjectManagementService.Infrastucture.Data.Repositories;
public class ProjectRepository : GenericRepository<Project>, IProjectRepository
{
    public ProjectRepository(IConfiguration configuration, IMongoClient mongoClient) :
        base(configuration, mongoClient, "ProjectCollectionName")
    { }

    public async Task<List<Project>> GetAllCustomerProjectsAsync(string customerId) 
    {
        return await GetFilteredAsync(project => project.CustomerId == customerId);
        //return (await projects.FindAsync(project => project.CustomerId == customerId)).ToList();
    }

    public async Task<Project?> GetProjectLeaderProject(string projectLeaderId) 
    {
        return await GetFirstAsync(task => task.LeadWorkerId == projectLeaderId);
        //return await projects.Find(task => task.LeadWorkerId == projectLeaderId).FirstAsync();
    }

    public async new Task<string> AddAsync(Project project)
    {
        await base.AddAsync(project);

        return project.Id;
    }
}
