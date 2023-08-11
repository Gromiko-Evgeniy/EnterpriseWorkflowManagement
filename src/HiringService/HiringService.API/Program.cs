using HiringService.Application.Abstractions;
using HiringService.Application.CQRS.CandidateCommands;
using HiringService.Infrastructure.Data;
using HiringService.Infrastructure.Data.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(
    o => o.UseNpgsql(
        builder.Configuration.GetConnectionString("HiringServiceDB"),
        b => b.MigrationsAssembly(builder.Configuration.GetSection("MigrationsAssembly").Get<string>())
    )
);

builder.Services.AddMediatR(typeof(AddCandidateCommand).Assembly);

builder.Services.AddTransient<ICandidateRepository, CandidateRepository>();
builder.Services.AddTransient<IHiringStageNameRepository, HiringStageNameRepository>();
builder.Services.AddTransient<IHiringStageRepository, HiringStageRepository>();
builder.Services.AddTransient<IWorkerRepository, WorkerRepository>();

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
