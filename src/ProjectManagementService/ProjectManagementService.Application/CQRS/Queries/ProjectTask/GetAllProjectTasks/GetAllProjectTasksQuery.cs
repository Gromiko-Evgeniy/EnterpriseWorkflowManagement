using MediatR;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectTaskQueries;

public sealed record GetAllProjectTasksQuery() : IRequest<List<ProjectTask>> { }
