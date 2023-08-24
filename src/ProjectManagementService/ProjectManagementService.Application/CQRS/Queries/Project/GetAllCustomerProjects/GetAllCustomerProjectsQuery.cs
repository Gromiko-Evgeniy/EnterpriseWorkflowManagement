using MediatR;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectQueries;

public sealed record GetAllCustomerProjectsQuery(string CustomerId) : IRequest<List<Project>> { }
