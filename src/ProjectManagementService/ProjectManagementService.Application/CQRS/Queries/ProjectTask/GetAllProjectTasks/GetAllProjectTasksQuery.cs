using MediatR;
using ProjectManagementService.Application.TaskDTOs;

namespace ProjectManagementService.Application.CQRS.ProjectTaskQueries;

public sealed record GetAllProjectTasksQuery() : IRequest<List<TaskShortInfoDTO>> { }
