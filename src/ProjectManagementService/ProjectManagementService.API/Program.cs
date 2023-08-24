using MediatR;
using ProjectManagementService.Application.Mapping;
using ProjectManagementService.Infrastucture.Data.Extensions;
using ProjectManagementService.Application.Validation;
using ProjectManagementService.Application.CQRS.Queries.Project.GetAllProjects;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabaseConfiguration(builder.Configuration);

builder.Services.AddMapping();

builder.Services.AddValidation();

builder.Services.AddRepositories();

builder.Services.AddMediatR(typeof(GetAllProjectsQuery).Assembly);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
