using IdentityService.Application.Services.Extensions;
using IdentityService.Infrastructure.Data.Extensions;
using IdentityService.Application.Configuration;
using IdentityService.Application.Kafka;
using IdentityService.Application.HangfireExtensions;
using Hangfire;
using IdentityService.Application.Middleware;
using IdentityService.Infrastructure.Data.AddTestingData;
using IdentityService.Infrastructure.Data;
using IdentityService.Application.Mapping;
using IdentityService.Application.Validation;
using IdentityService.Application.RepositoryAbstractions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabaseConfiguration(builder.Configuration);

builder.Services.AddMapping();

builder.Services.AddValidation();

builder.Services.AddRepositories();

builder.Services.AddServices();
builder.Services.AddKafkaProducer();

builder.Services.AddHangfire(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddAuthOptions(builder.Configuration);
builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(SwaggerAuthConfiguration.Configure);

var app = builder.Build();

app.UseExceptionHandlingMiddleware();

using var scope = app.Services.CreateScope();

var workerRepository = scope.ServiceProvider.GetRequiredService<IWorkerRepository>();
var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
WorkersActivityCheck.AddCheckWorkersActivityRecurringJob(workerRepository, recurringJobManager);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    await TestingDataContainer.AddTestingData(context);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
