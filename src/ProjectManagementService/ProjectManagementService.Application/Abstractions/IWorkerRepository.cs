using HiringService.Application.Abstractions;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.Abstractions;

public interface IWorkerRepository : IGenericRepository<Worker>
{
    public Task<string> AddAsync(Worker worker);

    public Task ChangeTaskAsync(string workerId, string taskId);
}
