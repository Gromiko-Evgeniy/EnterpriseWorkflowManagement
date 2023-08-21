using IdentityService.Application.DTOs;

namespace IdentityService.Application.Abstractions.ServiceAbstractions.TokenServices;

public interface IEntityTokenService
{
    public Task<string> GetTokenAsync(LogInData data);
}