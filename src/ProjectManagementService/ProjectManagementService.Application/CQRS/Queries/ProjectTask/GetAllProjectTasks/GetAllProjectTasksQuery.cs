using MediatR;
using ProjectManagementService.Application.DTOs.ProjectTaskDTOs;

namespace ProjectManagementService.Application.CQRS.ProjectTaskQueries;

public sealed record GetAllProjectTasksQuery() : IRequest<List<TaskShortInfoDTO>> { }
