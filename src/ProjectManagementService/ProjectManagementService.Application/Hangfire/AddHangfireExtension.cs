using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectManagementService.Application.Hangfire;

public static class AddHangfireExtension
{
    public static IServiceCollection AddHangfire(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Hangfire");

        services.AddHangfire(h => h.UseSqlServerStorage(connectionString));

        return services;
    }
}
