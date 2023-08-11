using HiringService.Domain.Entities;

namespace HiringService.Application.Abstractions;

public interface IWorkerRepository
{
    public Task<List<Worker>> GetAllAsync();

    public Task<Worker> GetByIdAsync(int workerId);

    public Task AddAsync(Worker worker);

    public Task RemoveAsync(int workerId);
}
