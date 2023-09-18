using ProjectManagementService.Domain.Entities;
using MongoDB.Driver;
using HiringService.Infrastructure.Data.Repositories;
using ProjectManagementService.Application.Configuration;
using Microsoft.Extensions.Options;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using MongoDB.Bson;

namespace ProjectManagementService.Infrastucture.Data.Repositories;

public class WorkerRepository : GenericRepository<Worker>, IWorkerRepository
{
    public WorkerRepository(IMongoClient mongoClient,
        IOptions<MongoDBConfiguration> DBconfiguration) :
        base(mongoClient, DBconfiguration)
    { }

    public async Task UpdateTaskAsync(string workerId, string taskId)
    {
        var update = new BsonDocument("$set", new BsonDocument("CurrentTaskId", taskId));

        await UpdateAsync(update, worker => worker.Id == workerId);
    }

    public async Task UpdateProjectAsync(string workerId, string projectId)
    {
        var update = new BsonDocument("$set", new BsonDocument("CurrentProjectId", projectId));

        await UpdateAsync(update, worker => worker.Id == workerId);
    }
}
