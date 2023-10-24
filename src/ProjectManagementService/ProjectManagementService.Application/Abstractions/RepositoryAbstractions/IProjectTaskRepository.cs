﻿using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.Abstractions.RepositoryAbstractions;

public interface IProjectTaskRepository : IGenericRepository<ProjectTask>
{
    public Task<List<ProjectTask>> GetByProjectIdAsync(string projectId);

    public Task<ProjectTask?> GetByWorkerIdAsync(string workerId);

    public Task SetNewWorker(string taskId, string workerId);

    public Task CancelAsync(string id);

    public Task MarkAsReadyToApproveAsync(string id);

    public Task MarkAsApproved(string id);

    public Task StartWorkingOnTask(string id);

    public Task FinishWorkingOnTask(string id);

    public Task UpdateNameAsync(string id, string name);

    public Task UpdateDescriptionAsync(string id, string description);

    public Task UpdateWorkerIdAsync(string id, string workerId);
}
