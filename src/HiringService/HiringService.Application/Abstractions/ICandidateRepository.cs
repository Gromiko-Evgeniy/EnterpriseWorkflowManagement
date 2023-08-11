using HiringService.Domain.Entities;

namespace HiringService.Application.Abstractions;
public interface ICandidateRepository
{
    public Task<List<Candidate>> GetAllAsync();

    public Task<Candidate> GetByIdAsync(int id);

    public Task<Candidate> GetByEmailAsync(string email);

    public Task AddAsync(Candidate candidate);

    public Task UpdateNameAsync(int id, string name);

    public Task UpdateCVAsync(int id, string CV);
}
