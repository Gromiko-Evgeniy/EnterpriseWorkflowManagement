using Microsoft.AspNetCore.Http;

namespace ProjectManagementService.Application.Abstractions.ServiceAbstractions;

public interface IJWTExtractorService
{
    public string ExtractClaimFromRequest(HttpRequest request, string claimType);

    public string ExtractClaimFromJWT(string JWT, string claimType);
}
