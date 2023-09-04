using HiringService.Application.Abstractions.RepositoryAbstractions;
using HiringService.Domain.Entities;

namespace HiringService.Infrastructure.Data.Repositories;

public class WorkerRepository : GenericRepository<Worker>, IWorkerRepository
{
    public WorkerRepository(DataContext context) : base(context) { }

    public async Task<Worker?> GetByEmailAsync(string email)
    {
        return await GetFirstAsync(s => s.Email == email);
    }
}
