using MediatR;
using ProjectManagementService.Application.ProjectTaskDTOs;

namespace ProjectManagementService.Application.CQRS.ProjectTaskCommands;

public sealed record AddProjectTaskCommand(AddProjectTaskDTO ProjectTaskDTO) : IRequest<string> { }
