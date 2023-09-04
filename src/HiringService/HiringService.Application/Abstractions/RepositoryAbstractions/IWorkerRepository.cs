using HiringService.Domain.Entities;

namespace HiringService.Application.Abstractions.RepositoryAbstractions;

public interface IWorkerRepository : IGenericRepository<Worker>
{
    public Task<Worker?> GetByEmailAsync(string email);
}
