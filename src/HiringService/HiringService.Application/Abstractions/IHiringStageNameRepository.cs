using HiringService.Domain.Entities;

namespace HiringService.Application.Abstractions;

public interface IHiringStageNameRepository
{
    public Task<List<HiringStageName>> GetAllAsync();

    public Task<HiringStageName> GetByIdAsync(int id);

    public Task<HiringStageName> GetByNameAsync(string name);

    public Task<int> AddAsync(string name);

    public Task UpdateNameAsync(int id, string name);

    public Task RemoveAsync(int id);
}
