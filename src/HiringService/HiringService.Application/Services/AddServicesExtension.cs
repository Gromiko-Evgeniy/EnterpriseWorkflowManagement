using HiringService.Application.Abstractions.ServiceAbstractions;
using Microsoft.Extensions.DependencyInjection;

namespace HiringService.Application.Services;
public static class AddServicesExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IGRPCService, GRPCService>();
        services.AddSingleton<IJWTExtractorService, JWTExtractorService>();

        return services;
    }
}