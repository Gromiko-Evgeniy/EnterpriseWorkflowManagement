using MediatR;

namespace ProjectManagementService.Application.CQRS.ProjectTaskCommands;

public sealed record CancelProjectTaskByIdCommand(string ProjectTaskId, string CustomerId) : IRequest{ } 
