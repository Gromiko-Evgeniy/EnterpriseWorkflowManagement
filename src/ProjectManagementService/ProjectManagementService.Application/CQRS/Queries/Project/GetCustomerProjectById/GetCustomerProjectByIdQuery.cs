using MediatR;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectQueries;

public sealed record GetCustomerProjectByIdQuery(string CustomerId, string ProjectId) : IRequest<Project> { }
