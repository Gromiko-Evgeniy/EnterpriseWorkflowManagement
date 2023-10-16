using MediatR;
using ProjectManagementService.Application.ProjectDTOs;

namespace ProjectManagementService.Application.CQRS.ProjectQueries;

public sealed record GetCustomerProjectByIdQuery(string CustomerId, string ProjectId) : IRequest<ProjectMainInfoDTO> { }
