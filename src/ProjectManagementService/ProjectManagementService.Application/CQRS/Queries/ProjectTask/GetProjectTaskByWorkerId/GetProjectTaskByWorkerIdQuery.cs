using MediatR;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectTaskQueries;

public sealed record GetProjectTaskByWorkerIdQuery(string WorkerId) : IRequest<ProjectTask> { }
