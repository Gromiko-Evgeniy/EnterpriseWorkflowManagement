using IdentityService.Application.Abstractions.ServiceAbstractions.TokenServices;
using IdentityService.Application.DTOs;
using IdentityService.Application.ServiceAbstractions;

namespace IdentityService.Application.Services.AuthenticationServices;

public class CustomerTokenService : ICustomerTokenService
{
    private readonly ICustomerService _customerService;
    private readonly ITokenGenerationService _tokenGenerationService;

    public CustomerTokenService(ICustomerService customerService, ITokenGenerationService tokenGenerationService)
    {
        _customerService = customerService;
        _tokenGenerationService = tokenGenerationService;
    }

    public async Task<string> GetTokenAsync(LogInData data)
    {
        var customer = await _customerService.GetByEmailAndPasswordAsync(data);

        var token = _tokenGenerationService.GetToken("Customer", customer.Email);

        return token;
    }
}
