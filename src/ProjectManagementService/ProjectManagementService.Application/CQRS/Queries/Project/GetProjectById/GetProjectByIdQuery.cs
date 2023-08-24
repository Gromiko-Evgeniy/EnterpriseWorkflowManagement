using MediatR;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectQueries;

public sealed record GetProjectByIdQuery(string Id) : IRequest<Project> { }
