using HiringService.Application.Abstractions;
using HiringService.Domain.Entities;

namespace HiringService.Infrastructure.Data.Repositories;

public class WorkerRepository : GenericRepository<Worker>, IWorkerRepository
{
    public WorkerRepository(DataContext context) : base(context) { }
}
