using HiringService.Application.Abstractions;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.Abstractions;

public interface IWorkerRepository : IGenericRepository<Worker>
{
    public new Task<string> AddAsync(Worker worker);
}
