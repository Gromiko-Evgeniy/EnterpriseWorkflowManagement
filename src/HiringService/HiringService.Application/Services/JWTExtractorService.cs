using HiringService.Application.Abstractions.ServiceAbstractions;
using HiringService.Application.Exceptions.Service;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
 
namespace HiringService.Application.Services;

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
