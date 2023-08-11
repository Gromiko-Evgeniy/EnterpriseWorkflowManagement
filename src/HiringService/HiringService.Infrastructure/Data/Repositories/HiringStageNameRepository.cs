using HiringService.Application.Abstractions;
using HiringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HiringService.Infrastructure.Data.Repositories;

public class HiringStageNameRepository : IHiringStageNameRepository
{
    private readonly DataContext context;

    public HiringStageNameRepository(DataContext context)
    {
        this.context = context;
    }

    public async Task<List<HiringStageName>> GetAllAsync()
    {
        return await context.HiringStageNames.AsNoTracking().ToListAsync();
    }

    public async Task<HiringStageName> GetByIdAsync(int id)
    {
        var stageName = await context.HiringStageNames.AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);

        if (stageName is null) return null; // trow ex 

        return stageName;
    }

    public async Task<HiringStageName> GetByNameAsync(string name)
    {
        var stageName = await context.HiringStageNames.AsNoTracking()
            .FirstOrDefaultAsync(s => s.Name == name);

        if (stageName is null) return null; // trow ex 

        return stageName;
    }

    public async Task<int> AddAsync(string name)
    {
        var stageName = await context.HiringStageNames.AsNoTracking()
            .FirstOrDefaultAsync(s => s.Name == name);

        if (stageName is not null) return stageName.Id;

        var newStageName = new HiringStageName() { Name = name };

        context.HiringStageNames.Add(newStageName);
        await context.SaveChangesAsync();

        return newStageName.Id;
    }

    public async Task UpdateNameAsync(int id, string name)
    {
        var stageName = await context.HiringStageNames.FirstOrDefaultAsync(s => s.Id == id);
        if (stageName is null) return; // trow ex 

        stageName.Name = name;
        await context.SaveChangesAsync();
    }

    public async Task RemoveAsync(int id)
    {
        var stageName = await GetByIdAsync(id);

        context.HiringStageNames.Remove(stageName);
        await context.SaveChangesAsync();
    }
}
