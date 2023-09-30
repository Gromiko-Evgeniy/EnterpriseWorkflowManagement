using MediatR;
using ProjectManagementService.Application.Mapping;
using ProjectManagementService.Infrastucture.Data.Extensions;
using ProjectManagementService.Application.Validation;
using ProjectManagementService.Application.CQRS.ProjectQueries;
using ProjectManagementService.Application.Configuration;
using HiringService.Application.Services;
using IdentityService.Application.Authentication;
using ProjectManagementService.Application.Kafka;
using HiringService.Application.Cache;
using ProjectManagementService.Application.Hangfire;
using ProjectManagementService.Application.Middleware;
using HiringService.Infrastructure.Data.AddTestingData;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.CQRS.MediatrPipeline;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabaseConnection(builder.Configuration);
builder.Services.AddMongoDBConfiguration(builder.Configuration);

builder.Services.AddRedisCaching(builder.Configuration);

builder.Services.AddMapping();

builder.Services.AddRepositories();

builder.Services.AddServices();

builder.Services.AddMediatR(typeof(GetAllProjectsQuery).Assembly);
builder.Services.AddMediatRPipelineBehaviors();

builder.Services.AddHangfire(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(SwaggerAuthConfiguration.Configure);

builder.Services.AddKafkaBGServices();

var app = builder.Build();

app.UseExceptionHandlingMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();

    var workerRepository = scope.ServiceProvider.GetRequiredService<IWorkerRepository>();
    var customerRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
    var projectRepository = scope.ServiceProvider.GetRequiredService<IProjectRepository>();
    var taskRepository = scope.ServiceProvider.GetRequiredService<IProjectTaskRepository>();

    await TestingDataContainer.AddTestingData(customerRepository,
        workerRepository, taskRepository, projectRepository);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
