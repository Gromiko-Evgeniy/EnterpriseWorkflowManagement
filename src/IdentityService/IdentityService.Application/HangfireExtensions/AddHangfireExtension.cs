using Hangfire;
using IdentityService.Application.HangfireExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Application.HangfireExtensions;

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
