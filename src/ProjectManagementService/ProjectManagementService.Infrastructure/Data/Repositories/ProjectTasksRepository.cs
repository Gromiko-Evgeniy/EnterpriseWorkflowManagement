using ProjectManagementService.Domain.Entities;
using ProjectManagementService.Application.Abstractions;
using ProjectManagementService.Domain.Enumerations;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ProjectManagementService.Infrastucture.Data.Repositories;

public class ProjectTasksRepository : IProjectTasksRepository
{
    private readonly IMongoCollection<ProjectTask> tasks;

    public ProjectTasksRepository(IConfiguration configuration, IMongoClient mongoClient) // "mongodb://localhost:27017"
    {
        var database = mongoClient.GetDatabase(configuration["DBConnectionSettings:DatabaseName"]);
        tasks = database.GetCollection<ProjectTask>(configuration["DBConnectionSettings:Collections:ProjectTaskCollectionName"]);
    }

    public async Task<List<ProjectTask>> GetAllAsync() 
    {
        return (await tasks.FindAsync(_ => true)).ToList();
    }

    public async Task<ProjectTask> GetByIdAsync(string id) 
    {
        return await tasks.Find(task => task.Id == id).FirstAsync();
    }

    public async Task<List<ProjectTask>> GetByProjectIdAsync(string projectId) 
    {
        return (await tasks.FindAsync(task => task.ProjectId == projectId)).ToList();
    }

    public async Task<string> AddAsync(ProjectTask task) 
    {
        await tasks.InsertOneAsync(task);
        return task.Id;
    } 

    public async Task CancelAsync(string id) 
    {
        var update = new BsonDocument("$set", new BsonDocument("status", ProjectTaskStatus.Canceled));
        await tasks.UpdateOneAsync(task => task.Id == id, update);
    }

    public async Task MarkAsApproved(string id)
    {
        var update = new BsonDocument("$set", new BsonDocument("status", ProjectTaskStatus.Approved));
        await tasks.UpdateOneAsync(task => task.Id == id, update);
    }

    public async Task MarkAsReadyToApproveAsync(string id)
    {
        var update = new BsonDocument("$set", new BsonDocument("status", ProjectTaskStatus.ReadyToApprove));
        await tasks.UpdateOneAsync(task => task.Id == id, update);
    }

    public async Task StartWorkingOnTask(string id)
    {
        var update = new BsonDocument("$set", new BsonDocument("startTime", DateTime.Now));
        await tasks.UpdateOneAsync(task => task.Id == id, update);
    }

    public async Task FinishWorkingOnTask(string id)
    {
        var update = new BsonDocument("$set", new BsonDocument("finishTime", DateTime.Now));
        await tasks.UpdateOneAsync(task => task.Id == id, update);
    }
}
