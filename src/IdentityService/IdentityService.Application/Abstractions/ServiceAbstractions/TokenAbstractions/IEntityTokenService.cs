using IdentityService.Application.DTOs;

namespace IdentityService.Application.TokenAbstractions;

public interface IEntityTokenService
{
    public Task<string> GetTokenAsync(LogInData data);
}
