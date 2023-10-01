using MediatR;
using ProjectManagementService.Application.ProjectDTOs;

namespace ProjectManagementService.Application.CQRS.ProjectQueries;

public sealed record GetProjectByIdQuery(string Id) : IRequest<ProjectMainInfoDTO> { }
