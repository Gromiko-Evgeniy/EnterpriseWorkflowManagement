using IdentityService.Application.Abstractions.ServiceAbstractions;
using IdentityService.Application.DTOs.CustomerDTOs;
using IdentityService.Application.DTOs.WorkerDTOs;

namespace IdentityService.Application.ServiceAbstractions;

public interface IWorkerService : IGenericService<GetWorkerDTO, AddWorkerDTO>
{
    public Task<List<GetWorkerDTO>> GetAllAsync();

    public Task<GetWorkerDTO> GetByEmailAsync(string email);

    public Task PromoteAsync(string email);

    public Task DemoteAsync(string email);

    public Task UpdateNameAsync(string email, string name);

    public Task DismissAsync(string email);

    public Task QuitAsync(string email, string password);
}
