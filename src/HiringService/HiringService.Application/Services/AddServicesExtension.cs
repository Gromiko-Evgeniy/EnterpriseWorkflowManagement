using Grpc.Net.Client;
using HiringService.Application.Abstractions.ServiceAbstractions;
using IdentityService.GRPC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HiringService.Application.Services;
public static class AddServicesExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<CandidateService.CandidateServiceClient>(services =>
        {
            var channel = GrpcChannel.ForAddress(configuration["GRPC:IdentityServerAddress"]!);
            return new CandidateService.CandidateServiceClient(channel);
        });

        services.AddSingleton<IGRPCService, GRPCService>();
        services.AddSingleton<IJWTExtractorService, JWTExtractorService>();

        return services;
    }
}