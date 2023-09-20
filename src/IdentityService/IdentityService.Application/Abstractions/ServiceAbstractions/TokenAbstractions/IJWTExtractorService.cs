using Microsoft.AspNetCore.Http;

namespace IdentityService.Application.TokenAbstractions;

public interface IJWTExtractorService
{
    public string ExtractClaim(HttpRequest request, string claimType);
}
