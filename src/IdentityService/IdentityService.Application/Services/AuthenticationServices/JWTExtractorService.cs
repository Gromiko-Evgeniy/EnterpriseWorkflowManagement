using IdentityService.Application.TokenAbstractions;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace IdentityService.Application.Services.AuthenticationServices;

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
