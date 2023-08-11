using HiringService.Application.Abstractions;
using HiringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HiringService.Infrastructure.Data.Repositories;

public class HiringStageRepository : IHiringStageRepository
{
    private readonly DataContext context;
    private readonly IWorkerRepository workerRepository;


    public HiringStageRepository(DataContext context, IWorkerRepository workerRepository)
    {
        this.context = context;
        this.workerRepository = workerRepository;
    }

    public async Task<List<HiringStage>> GetAllAsync()
    {
        return await context.HiringStages.AsNoTracking().ToListAsync();
    }

    public async Task<HiringStage> GetByIdAsync(int id)
    {
        var stage = await context.HiringStages.AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);

        if (stage is null) return null; // trow ex 

        return stage;
    }

    public async Task<List<HiringStage>> GetByIntervierIdAsync(int intervierId)
    {
        var stages = await context.HiringStages.AsNoTracking().ToListAsync();

        var intervierStages = stages.Where(s => s.IntervierId == intervierId).ToList();

        return intervierStages;
    }
    public async Task AddAsync(HiringStage stage)
    {
        context.HiringStages.Add(stage);
        await context.SaveChangesAsync();
    }

    public async Task AddIntervierAsync(int intervierId, int stageId)
    {
        var worker = await workerRepository.GetByIdAsync(intervierId);

        var stage = await context.HiringStages.FirstOrDefaultAsync(s => s.Id == stageId);
        if (stage is null) return; // trow ex 

        stage.Intervier = worker;
        await context.SaveChangesAsync();
    }

    public async Task MarkAsPassedSuccessfullyAsync(int intervierId, int stageId)
    {
        var worker = await workerRepository.GetByIdAsync(intervierId);

        var stage = await context.HiringStages.FirstOrDefaultAsync(s => s.Id == stageId);
        if (stage is null) return; // trow ex 

        if (stage.IntervierId != intervierId) return; // trow ex 

        stage.PassedSuccessfully = true;
        await context.SaveChangesAsync();
    }
}