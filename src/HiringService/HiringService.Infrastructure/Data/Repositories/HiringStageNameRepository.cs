using HiringService.Application.Abstractions;
using HiringService.Domain.Entities;

namespace HiringService.Infrastructure.Data.Repositories;

public class HiringStageNameRepository : GenericRepository<HiringStageName>, IHiringStageNameRepository
{
    public HiringStageNameRepository(DataContext context) : base(context) { }

    public async Task<HiringStageName> GetByNameAsync(string name)
    {
        return await GetFirstAsync(s => s.Name == name);
    }

    public async new Task<int> AddAsync(HiringStageName stageName)
    {
        var newStageName = await base.AddAsync(stageName);

        return newStageName.Id;
    }
}
