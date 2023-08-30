using IdentityService.Application.Abstractions.ServiceAbstractions.TokenServices;
using IdentityService.Application.DTOs;
using IdentityService.Application.ServiceAbstractions;

namespace IdentityService.Application.Services.AuthenticationServices;

public class CandidateTokenService : ICandidateTokenService
{
    private readonly ICandidateService _candidateService;
    private readonly ITokenGenerationService _tokenGenerationService;

    public CandidateTokenService(ICandidateService candidateService, ITokenGenerationService tokenGenerationService)
    {
        _candidateService = candidateService;
        _tokenGenerationService = tokenGenerationService;
    }

    public async Task<string> GetTokenAsync(LogInData data)
    {
        var candidate = await _candidateService.GetByEmailAndPasswordAsync(data);

        var token = _tokenGenerationService.GetToken("Candidate", candidate.Email);

        return token;
    }
}
