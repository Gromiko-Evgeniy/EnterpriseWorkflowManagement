using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HiringService.Application.MediatrPipeline;

public static class AddMediatRPipelineBehaviorsExtension
{
    public static IServiceCollection AddMediatRPipelineBehaviors(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
