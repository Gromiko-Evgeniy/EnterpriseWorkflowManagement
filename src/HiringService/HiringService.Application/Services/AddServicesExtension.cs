using HiringService.Application.Abstractions.ServiceAbstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HiringService.Application.Services;
public static class AddServicesExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IGRPCService, GRPCService>();
        services.AddSingleton<IJWTExtractorService, JWTExtractorService>();

        services.Configure<IdentityGRPCServiceAddress>(configuration.GetSection("GRPC"));

        return services;
    }
}