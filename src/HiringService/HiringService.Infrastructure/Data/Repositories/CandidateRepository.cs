using HiringService.Application.Abstractions;
using HiringService.Domain.Entities;

namespace HiringService.Infrastructure.Data.Repositories;

public class CandidateRepository : GenericRepository<Candidate>, ICandidateRepository
{
    public CandidateRepository(DataContext context) : base(context) { }

    public async Task<Candidate?> GetByEmailAsync(string email)
    {
        return await GetFirstAsync(s => s.Email == email);
    }
}
