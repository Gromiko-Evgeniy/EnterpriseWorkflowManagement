using ProjectManagementService.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ProjectManagementService.Application.Abstractions;

namespace ProjectManagementService.Infrastucture.Data.Repositories;
public class ProjectsRepository : IProjectsRepository
{
    private readonly IMongoCollection<Project> projects;

    public ProjectsRepository(IConfiguration configuration, IMongoClient mongoClient)
    {
        var databaseName = configuration["DBConnectionSettings:DatabaseName"];
        var collectionName = configuration["DBConnectionSettings:Collections:ProjectCollectionName"];

        var database = mongoClient.GetDatabase(databaseName);
        projects = database.GetCollection<Project>(collectionName);
    }

    public async Task<List<Project>> GetAllAsync() 
    {
        return (await projects.FindAsync(_ => true)).ToList();
    }

    public async Task<Project> GetByIdAsync(string id) 
    {
        return await projects.Find(task => task.Id == id).FirstAsync();
    }

    public async Task<List<Project>> GetAllCustomerProjectsAsync(string customerId) 
    {
        return (await projects.FindAsync(project => project.CustomerId == customerId)).ToList();
    }

    public async Task<Project> GetProjectLeaderProject(string projectLeaderId) 
    {
        return await projects.Find(task => task.LeadWorkerId == projectLeaderId).FirstAsync();
    }

    public async Task<string> AddAsync(Project project) 
    {
        await projects.InsertOneAsync(project);
        return project.Id;
    }
}
