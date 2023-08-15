using HiringService.Application.Abstractions;
using HiringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HiringService.Infrastructure.Data.Repositories;

public class HiringStageRepository : GenericRepository<HiringStage>, IHiringStageRepository
{
    public HiringStageRepository(DataContext context) : base(context) { }

    public async Task<List<HiringStage>> GetByIntervierIdAsync(int intervierId)
    {
        return await GetFilteredAsync(s => s.IntervierId == intervierId);
    }

    public async new Task<int> AddAsync(HiringStage stage)
    {
        var newStage = await base.AddAsync(stage);

        return newStage.Id;
    }
}