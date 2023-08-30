using MediatR;
using ProjectManagementService.Application.DTOs.ProjectTaskDTOs;

namespace ProjectManagementService.Application.CQRS.ProjectTaskQueries;

public sealed record GetProjectTasksByProjectIdQuery(string ProjectId) : IRequest<List<TaskShortInfoDTO>> { }
