using MediatR;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectQueries;

public sealed record GetProjectByLeaderIdQuery(string ProjectLeaderId) : IRequest<Project> { }
