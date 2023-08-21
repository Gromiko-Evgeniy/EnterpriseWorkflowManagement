using IdentityService.Application.RepositoryAbstractions;
using IdentityService.Domain.Entities;

namespace IdentityService.Infrastructure.Data.Repositories;

public class CandidateRepository : GenericRepository<Candidate>, ICandidateRepository
{
    public CandidateRepository(DataContext context) : base(context) { }

    public async override Task<Candidate?> GetByEmailAsync(string email)
    {
        return await GetFirstAsync(s => s.Email == email);
    }
}
