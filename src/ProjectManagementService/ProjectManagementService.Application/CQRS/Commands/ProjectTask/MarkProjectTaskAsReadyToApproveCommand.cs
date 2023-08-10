using MediatR;

namespace ProjectManagementService.Application.CQRS.ProjectTaskCommands;

public sealed record MarkProjectTaskAsReadyToApproveCommand(string ProjectTaskId, string WorkerId) : IRequest { }