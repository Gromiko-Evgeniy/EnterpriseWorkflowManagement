using HiringService.Application.Abstractions;
using HiringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HiringService.Infrastructure.Data.Repositories;

public class CandidateRepository : ICandidateRepository
{
    private readonly DataContext context;

    public CandidateRepository(DataContext context)
    {
        this.context = context;
    }

    public async Task<List<Candidate>> GetAllAsync()
    {
        return await context.Candidates.AsNoTracking().ToListAsync();
    }
    public async Task<Candidate> GetByIdAsync(int id)
    {
        var stageName = await context.Candidates.AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);

        if (stageName is null) return null; // trow ex 

        return stageName;
    }

    public async Task<Candidate> GetByEmailAsync(string email)
    {
        var stageName = await context.Candidates.AsNoTracking()
            .FirstOrDefaultAsync(s => s.Email == email);

        if (stageName is null) return null; // trow ex 

        return stageName;
    }

    public async Task AddAsync(Candidate candidate)
    {
        bool alreadyExists = await context.Candidates.AsNoTracking()
            .AnyAsync(s => s.Email == candidate.Email);

        if (alreadyExists) return; // trow ex "alredy exists"

        context.Candidates.Add(candidate);
        await context.SaveChangesAsync();
    }

    public async Task UpdateCVAsync(int id, string CV)
    {
        var candidate = await GetByIdAsync(id);

        candidate.CV = CV;

        context.Candidates.Update(candidate);
        await context.SaveChangesAsync();
    }

    public async Task UpdateNameAsync(int id, string name)
    {
        var candidate = await GetByIdAsync(id);

        candidate.Name = name;

        context.Candidates.Update(candidate);
        await context.SaveChangesAsync();
    }
}
