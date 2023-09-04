using HiringService.Application.Abstractions.ServiceAbstractions;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
 
namespace HiringService.Application.Services;

public class JWTExtractorService : IJWTExtractorService
{
    public string ExtractClaim(HttpRequest request, string claimType)
    {
        var token = request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

        var value = jwtToken.Claims.FirstOrDefault(claim => claim.Type == claimType)?.Value;

        return value;
    }
}
