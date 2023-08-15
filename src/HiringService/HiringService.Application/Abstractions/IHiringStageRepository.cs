using HiringService.Application.DTOs.HiringStageDTOs;
using HiringService.Domain.Entities;

namespace HiringService.Application.Abstractions;

public interface IHiringStageRepository : IGenericRepository<HiringStage>
{
    public Task<List<HiringStage>> GetByIntervierIdAsync(int intervierId);

    //public Task AddIntervierAsync(int intervierId, int stageId);

    //public Task MarkAsPassedSuccessfullyAsync(int intervierId, int stageId);

    public new Task<int> AddAsync(HiringStage stage);
}
