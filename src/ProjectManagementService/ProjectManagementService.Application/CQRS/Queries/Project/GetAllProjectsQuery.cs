using ProjectManagementService.Domain.Entities;
using MediatR;

namespace ProjectManagementService.Application.CQRS.ProjectQueries;

public sealed record GetAllProjectsQuery : IRequest<List<Project>> { }