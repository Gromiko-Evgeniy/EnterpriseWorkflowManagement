using MediatR;

namespace ProjectManagementService.Application.CQRS.ProjectTaskCommands;

public sealed record MarkProjectTaskAsReadyToApproveCommand(string WorkerId) : IRequest { }