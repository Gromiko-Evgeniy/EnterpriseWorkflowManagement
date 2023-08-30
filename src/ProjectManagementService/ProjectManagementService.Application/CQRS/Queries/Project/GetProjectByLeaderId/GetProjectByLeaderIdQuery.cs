using MediatR;
using ProjectManagementService.Application.DTOs.ProjectDTOs;

namespace ProjectManagementService.Application.CQRS.ProjectQueries;

public sealed record GetProjectByLeaderIdQuery(string ProjectLeaderId) : IRequest<ProjectMainInfoDTO> { }
