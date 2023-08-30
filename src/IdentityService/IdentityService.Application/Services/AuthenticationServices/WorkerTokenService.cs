using IdentityService.Application.Abstractions.ServiceAbstractions.TokenServices;
using IdentityService.Application.DTOs;
using IdentityService.Application.ServiceAbstractions;

namespace IdentityService.Application.Services.AuthenticationServices;

public class WorkerTokenService : IWorkerTokenService
{
    private readonly IWorkerService _workerService;
    private readonly ITokenGenerationService _tokenGenerationService;

    public WorkerTokenService(IWorkerService workerService, ITokenGenerationService tokenGenerationService)
    {
        _workerService = workerService;
        _tokenGenerationService = tokenGenerationService;
    }

    public async Task<string> GetTokenAsync(LogInData data)
    {
        var worker = await _workerService.GetByEmailAndPasswordAsync(data);

        var token = _tokenGenerationService.GetToken(worker.Position.ToString(), worker.Email);

        return token;
    }
}
