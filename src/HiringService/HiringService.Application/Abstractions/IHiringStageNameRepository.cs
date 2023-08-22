using HiringService.Domain.Entities;

namespace HiringService.Application.Abstractions;

public interface IHiringStageNameRepository : IGenericRepository<HiringStageName>
{
    public Task<HiringStageName?> GetByNameAsync(string name);
}
