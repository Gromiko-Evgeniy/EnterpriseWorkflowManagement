using MediatR;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectQueries;

public sealed record GetProjectLeaderProjectQuery(string ProjectLeaderId) : IRequest<Project> { }
