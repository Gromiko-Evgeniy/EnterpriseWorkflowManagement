using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.Abstractions.RepositoryAbstractions;

public interface IWorkerRepository : IGenericRepository<Worker>
{
    public new Task<string> AddAsync(Worker worker);
}
