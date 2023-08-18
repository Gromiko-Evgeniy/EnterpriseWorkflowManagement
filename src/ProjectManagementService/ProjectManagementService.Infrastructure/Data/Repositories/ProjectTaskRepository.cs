using ProjectManagementService.Domain.Entities;
using ProjectManagementService.Application.Abstractions;
using ProjectManagementService.Domain.Enumerations;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using HiringService.Infrastructure.Data.Repositories;

namespace ProjectManagementService.Infrastucture.Data.Repositories;

public class ProjectTaskRepository : GenericRepository<ProjectTask>, IProjectTaskRepository
{
    private readonly IMongoCollection<ProjectTask> tasks;

    public ProjectTaskRepository(IConfiguration configuration, IMongoClient mongoClient) :
        base(configuration, mongoClient, "ProjectTaskCollectionName") { }

    public async Task<List<ProjectTask>> GetByProjectIdAsync(string projectId) 
    {
        return await GetFilteredAsync(task => task.ProjectId == projectId);
    }

    public async new Task<string> AddAsync(ProjectTask task)
    {
        await base.AddAsync(task);

        return task.Id;
    }

    public async Task CancelAsync(string id) 
    {
        await UpdatePropertyAsync(id, "status", ProjectTaskStatus.Canceled);

        //var update = new BsonDocument("$set", new BsonDocument("status", ProjectTaskStatus.Canceled));
        //await tasks.UpdateOneAsync(task => task.Id == id, update);
    }

    public async Task MarkAsApproved(string id)
    {
        await UpdatePropertyAsync(id, "status", ProjectTaskStatus.Approved);

        //var update = new BsonDocument("$set", new BsonDocument("status", ProjectTaskStatus.Approved));
        //await tasks.UpdateOneAsync(task => task.Id == id, update);
    }

    public async Task MarkAsReadyToApproveAsync(string id)
    {
        await UpdatePropertyAsync(id, "status", ProjectTaskStatus.ReadyToApprove);
    }

    public async Task StartWorkingOnTask(string id)
    {
        await UpdatePropertyAsync(id, "startTime", DateTime.Now);
    }

    public async Task FinishWorkingOnTask(string id)
    {
        await UpdatePropertyAsync(id, "finishTime", DateTime.Now);
    }

    private async Task UpdatePropertyAsync(string id, string propertuName, BsonValue value)
    {
        var update = new BsonDocument("$set", new BsonDocument(propertuName, value));

        await UpdateAsync(update, task => task.Id == id);
    }
}
