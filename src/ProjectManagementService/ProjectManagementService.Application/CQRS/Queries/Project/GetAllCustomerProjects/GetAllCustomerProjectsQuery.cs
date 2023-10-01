using MediatR;
using ProjectManagementService.Application.ProjectDTOs;

namespace ProjectManagementService.Application.CQRS.ProjectQueries;

public sealed record GetAllCustomerProjectsQuery(string CustomerId) : IRequest<List<ProjectShortInfoDTO>> { }
