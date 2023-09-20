using Microsoft.AspNetCore.Http;

namespace ProjectManagementService.Application.Abstractions.ServiceAbstractions;

public interface IJWTExtractorService
{
    public string ExtractClaim(HttpRequest request, string claimType);
}
