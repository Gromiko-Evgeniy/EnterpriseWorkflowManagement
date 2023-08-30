using IdentityService.Application.Abstractions.ServiceAbstractions.TokenServices;
using IdentityService.Application.ServiceAbstractions;
using IdentityService.Application.Services.AuthenticationServices;
using IdentityService.Application.Services.EntityServices;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Application.Services.Extensions;

public static class AddServicesExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICandidateService, CandidateService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IWorkerService, WorkerService>();

        services.AddScoped<ITokenGenerationService, TokenGenerationService>();

        services.AddScoped<ICandidateTokenService, CandidateTokenService>();
        services.AddScoped<ICustomerTokenService, CustomerTokenService>();
        services.AddScoped<IWorkerTokenService, WorkerTokenService>();

        return services;
    }
}
