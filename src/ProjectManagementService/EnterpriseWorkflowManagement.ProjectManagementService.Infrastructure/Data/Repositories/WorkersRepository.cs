using ProjectManagementService.Domain.Entities;
using ProjectManagementService.Application.Abstractions;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ProjectManagementService.Infrastucture.Data.Repositories;
public class WorkersRepository : IWorkersRepository
{
    private readonly IMongoCollection<Worker> workers;
    private readonly IProjectTasksRepository tasksRepository;

    public WorkersRepository(IProjectTasksRepository tasksRepository, IConfiguration configuration, IMongoClient mongoClient)
    {
        this.tasksRepository = tasksRepository;

        var database = mongoClient.GetDatabase(configuration["DBConnectionSettings:DatabaseName"]);
        workers = database.GetCollection<Worker>(configuration["DBConnectionSettings:Collections:WorkerCollectionName"]);
    }

    public async Task<List<Worker>> GetAllAsync()
    {
        return (await workers.FindAsync(_ => true)).ToList();
    }

    public async Task<Worker> GetByIdAsync(string id)
    {
        return await workers.Find(task => task.Id == id).FirstAsync();
    }

    public async Task AddAsync(Worker worker)
    {
        await workers.InsertOneAsync(worker);
    }
     
    public async Task ChangeTaskAsync(string workerId, string taskId)
    {
        var task = await tasksRepository.GetByIdAsync(taskId);

        if (task is null) return; // throw ex

        var newTaskDocument = new BsonDocument()
        {
            { "currentTaskId", task.Id },
            { "currentProjectId", task.ProjectId }
        };

        var update = new BsonDocument("$set", newTaskDocument);
        await workers.UpdateOneAsync(worker => worker.Id == workerId, update);
    }

    public async Task RemoveAsync(string workerId)
    {
        await workers.DeleteOneAsync(worker => worker.Id == workerId);
    } 
}
