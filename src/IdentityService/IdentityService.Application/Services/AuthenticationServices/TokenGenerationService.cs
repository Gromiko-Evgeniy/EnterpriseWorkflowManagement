using IdentityService.Application.Abstractions.ServiceAbstractions.TokenServices;
using IdentityService.Application.RepositoryAbstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityService.Application.Services.AuthenticationServices;

public class TokenGenerationService : ITokenGenerationService
{
    private readonly AuthOptions _authOptions;

    public TokenGenerationService(IConfiguration configuration, ICandidateRepository candidateRepository,
        ICustomerRepository customerRepository, IWorkerRepository workerRepository)
    {
        _authOptions = configuration.GetSection("Auth").Get<AuthOptions>(); ;
    }

    public async Task<string> GetTokenAsync(string role, string email)
    {
        var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim("role", role)
        };

        var jwt = new JwtSecurityToken(
            _authOptions.Issuer,
            _authOptions.Audience,
            claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(_authOptions.TokenLifeTime)),
            signingCredentials: new SigningCredentials(
                _authOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}