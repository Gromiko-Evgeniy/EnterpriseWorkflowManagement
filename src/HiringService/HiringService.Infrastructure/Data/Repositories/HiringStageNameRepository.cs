using HiringService.Application.Abstractions.RepositoryAbstractions;
using HiringService.Domain.Entities;

namespace HiringService.Infrastructure.Data.Repositories;

public class HiringStageNameRepository : GenericRepository<HiringStageName>, IHiringStageNameRepository
{
    public HiringStageNameRepository(DataContext context) : base(context) { }

    public async Task<HiringStageName?> GetByNameAsync(string name)
    {
        return await GetFirstAsync(s => s.Name == name);
    }
}
