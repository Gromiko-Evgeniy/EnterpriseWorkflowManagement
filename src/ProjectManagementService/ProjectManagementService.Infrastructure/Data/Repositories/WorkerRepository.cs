using ProjectManagementService.Domain.Entities;
using ProjectManagementService.Application.Abstractions;
using MongoDB.Driver;
using HiringService.Infrastructure.Data.Repositories;
using ProjectManagementService.Application.Configuration;
using Microsoft.Extensions.Options;

namespace ProjectManagementService.Infrastucture.Data.Repositories;

public class WorkerRepository : GenericRepository<Worker>, IWorkerRepository
{
    public WorkerRepository(IMongoClient mongoClient,
        IOptions<MongoDBConfiguration> DBconfiguration) :
        base(mongoClient, DBconfiguration)
    { }
}
