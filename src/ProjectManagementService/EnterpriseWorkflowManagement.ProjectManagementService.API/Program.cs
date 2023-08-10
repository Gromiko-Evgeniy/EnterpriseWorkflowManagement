using MediatR;
using MongoDB.Driver;
using ProjectManagementService.Application.Abstractions;
using ProjectManagementService.Application.CQRS.ProjectQueries;
using ProjectManagementService.Infrastucture.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connetionString = builder.Configuration.GetSection("DBConnectionSettings:ConnetionString").Get<string>();
builder.Services.AddSingleton<IMongoClient>(service => new MongoClient(connetionString));

builder.Services.AddSingleton<IProjectsRepository, ProjectsRepository>();
builder.Services.AddSingleton<IProjectTasksRepository, ProjectTasksRepository>();
builder.Services.AddSingleton<IWorkersRepository, WorkersRepository>();

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
