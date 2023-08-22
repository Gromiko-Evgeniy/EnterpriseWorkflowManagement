using HiringService.Application.Abstractions;
using HiringService.Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HiringService.Infrastructure.Data.Extensions;

public static class AddRepositoriesExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICandidateRepository, CandidateRepository>();
        services.AddScoped<IHiringStageRepository, HiringStageRepository>();
        services.AddScoped<IHiringStageNameRepository, HiringStageNameRepository>();
        services.AddScoped<IWorkerRepository, WorkerRepository>();

        return services;
    }
}
