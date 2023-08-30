﻿using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ProjectManagementService.Application.Mapping;

public static class AddMappingExtension
{
    public static IServiceCollection AddMapping(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
}
