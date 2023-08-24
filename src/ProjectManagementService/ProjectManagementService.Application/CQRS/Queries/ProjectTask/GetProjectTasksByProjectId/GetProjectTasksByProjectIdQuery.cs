using MediatR;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectTaskQueries;

public sealed record GetProjectTasksByProjectIdQuery(string ProjectId) : IRequest<List<ProjectTask>> { }
