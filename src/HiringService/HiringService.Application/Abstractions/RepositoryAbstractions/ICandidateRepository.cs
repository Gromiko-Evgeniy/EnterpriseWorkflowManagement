using HiringService.Domain.Entities;

namespace HiringService.Application.Abstractions.RepositoryAbstractions;

public interface ICandidateRepository : IGenericRepository<Candidate>
{
    public Task<Candidate?> GetByEmailAsync(string email);
}
