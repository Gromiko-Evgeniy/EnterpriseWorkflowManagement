using IdentityService.GRPC.Services;
using ProjectManagementService.Application.Mapping;
using IdentityService.Application.Services.Extensions;
using IdentityService.Infrastructure.Data.Extensions;
using ProjectManagementService.Application.Validation;
using IdentityService.Application.Kafka;
using IdentityService.Application.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabaseConfiguration(builder.Configuration);

builder.Services.AddMapping();

builder.Services.AddValidation();

builder.Services.AddRepositories();

builder.Services.AddServices();
builder.Services.AddAuthOptions(builder.Configuration);
builder.Services.AddKafkaProducer();

builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<CandidateGRPCService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
