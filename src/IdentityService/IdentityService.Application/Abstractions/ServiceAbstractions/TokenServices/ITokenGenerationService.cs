namespace IdentityService.Application.Abstractions.ServiceAbstractions.TokenServices;

public interface ITokenGenerationService
{
    public string GetToken(string role, string email);
}
