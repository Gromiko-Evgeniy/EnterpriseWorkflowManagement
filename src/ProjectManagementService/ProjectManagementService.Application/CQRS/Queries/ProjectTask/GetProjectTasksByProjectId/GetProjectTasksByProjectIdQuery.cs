using MediatR;
using ProjectManagementService.Application.TaskDTOs;

namespace ProjectManagementService.Application.CQRS.ProjectTaskQueries;

public sealed record GetProjectTasksByProjectIdQuery(string ProjectId) : IRequest<List<TaskShortInfoDTO>> { }
