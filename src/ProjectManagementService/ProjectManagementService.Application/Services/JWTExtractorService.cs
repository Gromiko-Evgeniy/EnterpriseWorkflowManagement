using Microsoft.AspNetCore.Http;
using ProjectManagementService.Application.Abstractions.ServiceAbstractions;
using ProjectManagementService.Application.Exceptions.Service;
using System.IdentityModel.Tokens.Jwt;

namespace ProjectManagementService.Application.Services;

public class JWTExtractorService : IJWTExtractorService
{
    public string ExtractClaim(HttpRequest request, string claimType)
    {
        var token = request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token is null) throw new NoEmailInJWTException();
        if (token == "") throw new EmptyEmailInJWTException();

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

        var value = jwtToken.Claims.FirstOrDefault(claim => claim.Type == claimType)?.Value;

        return value;
    }
}
