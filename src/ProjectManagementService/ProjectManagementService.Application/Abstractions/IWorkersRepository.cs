using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.Abstractions;
public interface IWorkersRepository
{
    public Task<List<Worker>> GetAllAsync();

    public Task<Worker> GetByIdAsync(string id);

    public Task AddAsync(Worker worker);

    public Task ChangeTaskAsync(string workerId, string taskId);

    public Task RemoveAsync(string workerId);
}
