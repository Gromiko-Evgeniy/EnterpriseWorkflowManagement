using IdentityService.Application.RepositoryAbstractions;
using IdentityService.Domain.Entities;

namespace IdentityService.Infrastructure.Data.Repositories;

public class WorkerRepository : GenericRepository<Worker>, IWorkerRepository
{
    public WorkerRepository(DataContext context) : base(context) { }
}
