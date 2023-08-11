using HiringService.Application.Abstractions;
using HiringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HiringService.Infrastructure.Data.Repositories;

public class WorkerRepository : IWorkerRepository
{
    private readonly DataContext context;

    public WorkerRepository(DataContext context)
    {
        this.context = context;
    }

    public async Task<List<Worker>> GetAllAsync()
    {
        return await context.Workers.AsNoTracking().ToListAsync();
    }

    public async Task<Worker> GetByIdAsync(int workerId)
    {
        var worker =  await context.Workers.AsNoTracking()
            .FirstOrDefaultAsync(w => w.Id == workerId);

        if (worker is null) return null; // trow ex 

        return worker;
    }

    public async Task AddAsync(Worker worker)
    {
        var alreadyExists = await context.Workers.AsNoTracking()
            .AnyAsync(w => w.Email == worker.Email);

        if (alreadyExists) return; // trow ex

        context.Workers.Add(worker);
        await context.SaveChangesAsync();
    }

    public async Task RemoveAsync(int workerId)
    {
        var worker = await context.Workers.FirstOrDefaultAsync(w => w.Id == workerId);

        if (worker is null) return; // trow ex 

        context.Workers.Remove(worker);
        await context.SaveChangesAsync();
    }
}