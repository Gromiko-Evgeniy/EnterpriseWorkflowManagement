namespace IdentityService.Application.Abstractions.ServiceAbstractions.TokenServices;

public interface ITokenGenerationService
{
    public Task<string> GetTokenAsync(string role, string email);
}
