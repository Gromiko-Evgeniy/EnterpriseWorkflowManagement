namespace IdentityService.Application.TokenAbstractions;

public interface ITokenGenerationService
{
    public string GetToken(string role, string email);
}
