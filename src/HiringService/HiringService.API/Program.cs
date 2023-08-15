using HiringService.Application.Abstractions;
using HiringService.Application.CQRS.CandidateCommands;
using HiringService.Infrastructure.Data.Extensions;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabaseConfiguration(builder.Configuration);

builder.Services.AddRepositories();

builder.Services.AddMediatR(typeof(AddCandidateCommand).Assembly);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
