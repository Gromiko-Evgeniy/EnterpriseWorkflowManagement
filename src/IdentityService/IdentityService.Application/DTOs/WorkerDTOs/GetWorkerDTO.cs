using IdentityService.Domain.Enumerations;

namespace IdentityService.Application.DTOs.WorkerDTOs;

public class GetWorkerDTO
{
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public Position Position { get; set; }
}
