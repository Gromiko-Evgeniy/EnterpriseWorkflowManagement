using ProjectManagementService.Application.Mapping;
using IdentityService.Application.Services.Extensions;
using IdentityService.Infrastructure.Data.Extensions;
using ProjectManagementService.Application.Validation;
using IdentityService.Application.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabaseConfiguration(builder.Configuration);

builder.Services.AddMapping();

builder.Services.AddValidation();

builder.Services.AddRepositories();

builder.Services.AddServices();

builder.Services.AddControllers();

builder.Services.AddAuthOptions(builder.Configuration);
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
