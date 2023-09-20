using Microsoft.AspNetCore.Http;

namespace HiringService.Application.Abstractions.ServiceAbstractions;

public interface IJWTExtractorService
{
    public string? ExtractClaim(HttpRequest request, string claimType);
}
