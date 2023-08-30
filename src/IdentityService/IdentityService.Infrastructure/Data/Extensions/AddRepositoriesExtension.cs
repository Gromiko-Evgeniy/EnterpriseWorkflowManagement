using IdentityService.Application.RepositoryAbstractions;
using IdentityService.Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Infrastructure.Data.Extensions;

public static class AddRepositoriesExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICandidateRepository, CandidateRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IWorkerRepository, WorkerRepository>();

        return services;
    }
}
