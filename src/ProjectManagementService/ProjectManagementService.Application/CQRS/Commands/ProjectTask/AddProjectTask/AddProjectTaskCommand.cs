using MediatR;
using ProjectManagementService.Application.TaskDTOs;

namespace ProjectManagementService.Application.CQRS.ProjectTaskCommands;

public sealed record AddProjectTaskCommand(AddProjectTaskDTO ProjectTaskDTO) : IRequest<string> { }
