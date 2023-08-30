using MediatR;
using ProjectManagementService.Application.DTOs.ProjectDTOs;

namespace ProjectManagementService.Application.CQRS.ProjectQueries;

public sealed record GetAllProjectsQuery : IRequest<List<ProjectShortInfoDTO>> { }
