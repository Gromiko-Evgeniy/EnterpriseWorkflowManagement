using HiringService.Domain.Entities;

namespace HiringService.Application.Abstractions;

public interface ICandidateRepository : IGenericRepository<Candidate>
{
    public Task<Candidate?> GetByEmailAsync(string email);

    public new Task<int> AddAsync(Candidate candidate);
}
