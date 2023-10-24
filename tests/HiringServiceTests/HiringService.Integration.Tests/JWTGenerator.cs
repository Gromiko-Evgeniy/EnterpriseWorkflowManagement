using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HiringService.Integration.Tests;
public class JWTGenerator
{
    private readonly AuthOptions _authOptions;

    public JWTGenerator()
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        var config = builder.Build();

        _authOptions = config.GetSection("Auth").Get<AuthOptions>();
    }

    public string GetToken(string role, string email)
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
