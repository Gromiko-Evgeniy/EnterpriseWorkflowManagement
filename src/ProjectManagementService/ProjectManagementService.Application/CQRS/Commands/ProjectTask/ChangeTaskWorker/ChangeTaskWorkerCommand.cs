using MediatR;

namespace ProjectManagementService.Application.CQRS.ProjectTaskCommands;

public sealed record ChangeTaskWorkerCommand(string WorkerId, string TaskId) : IRequest { }
