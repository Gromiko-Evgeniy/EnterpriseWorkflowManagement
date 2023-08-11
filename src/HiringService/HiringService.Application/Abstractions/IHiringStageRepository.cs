using HiringService.Application.DTOs.HiringStageDTOs;
using HiringService.Domain.Entities;

namespace HiringService.Application.Abstractions;

public interface IHiringStageRepository
{
    public Task<List<HiringStage>> GetAllAsync();

    public Task<HiringStage> GetByIdAsync(int id);

    public Task<List<HiringStage>> GetByIntervierIdAsync(int intervierId);

    public Task AddIntervierAsync(int intervierId, int stageId);

    public Task MarkAsPassedSuccessfullyAsync(int intervierId, int stageId);

    public Task AddAsync(HiringStage stage);
}
