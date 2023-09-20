using HiringService.Application.Cache;
using HiringService.Application.CQRS.CandidateCommands;
using HiringService.Application.CQRS.MediatrPipeline;
using HiringService.Application.Kafka;
using HiringService.Application.Mapping;
using HiringService.Application.Services;
using HiringService.Application.Validation;
using HiringService.Infrastructure.Data.Extensions;
using IdentityService.Application.Authentication;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabaseConfiguration(builder.Configuration);

builder.Services.AddRedisCaching(builder.Configuration);

builder.Services.AddRepositories();

builder.Services.AddServices(builder.Configuration);

builder.Services.AddMapping();

builder.Services.AddMediatR(typeof(AddCandidateCommand).Assembly);
builder.Services.AddMediatRPipelineBehaviors();

builder.Services.AddControllers();

builder.Services.AddKafkaBGServices();

builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(SwaggerAuthConfiguration.Configure);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
