using ProjectManagementService.Domain.Entities;
using ProjectManagementService.Domain.Enumerations;
using MongoDB.Driver;
using HiringService.Infrastructure.Data.Repositories;
using ProjectManagementService.Application.Configuration;
using Microsoft.Extensions.Options;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;

namespace ProjectManagementService.Infrastucture.Data.Repositories;

public class ProjectTaskRepository : GenericRepository<ProjectTask>, IProjectTaskRepository
{
    public ProjectTaskRepository(IMongoClient mongoClient,
        IOptions<MongoDBConfiguration> DBconfiguration) :
        base(mongoClient, DBconfiguration)
    { }

    public async Task<List<ProjectTask>> GetByProjectIdAsync(string projectId) 
    {
        return await GetFilteredAsync(task => task.ProjectId == projectId);
    }

    public async Task<ProjectTask?> GetByWorkerIdAsync(string workerId)
    {
        return await GetFirstAsync(task => task.WorkerId == workerId);
    }

    public async Task SetNewWorker(string taskId, string workerId)
    {
        await UpdatePropertyAsync(taskId, "workerId", workerId);
    }

    public async Task CancelAsync(string id) 
    {
        await UpdatePropertyAsync(id, "status", ProjectTaskStatus.Canceled);
    }

    public async Task MarkAsApproved(string id)
    {
        await UpdatePropertyAsync(id, "status", ProjectTaskStatus.Approved);
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

    public async Task UpdateNameAsync(string id, string name)
    {
        await UpdatePropertyAsync(id, "name", name);
    }

    public async Task UpdateDescriptionAsync(string id, string description)
    {
        await UpdatePropertyAsync(id, "description", description);
    }
}
