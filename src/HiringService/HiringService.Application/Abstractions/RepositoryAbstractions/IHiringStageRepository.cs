using HiringService.Application.DTOs.HiringStageDTOs;
using HiringService.Domain.Entities;

namespace HiringService.Application.Abstractions.RepositoryAbstractions;

public interface IHiringStageRepository : IGenericRepository<HiringStage>
{
    public Task<List<HiringStage>> GetByIntervierIdAsync(int intervierId);
}
