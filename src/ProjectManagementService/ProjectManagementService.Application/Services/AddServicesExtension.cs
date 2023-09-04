using Microsoft.Extensions.DependencyInjection;
using ProjectManagementService.Application.Abstractions.ServiceAbstractions;
using ProjectManagementService.Application.Services;

namespace HiringService.Application.Services;
public static class AddServicesExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IJWTExtractorService, JWTExtractorService>();

        return services;
    }
}