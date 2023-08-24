using MediatR;

namespace ProjectManagementService.Application.CQRS.ProjectTaskCommands;

public sealed record MarkProjectTaskAsApprovedCommand(string ProjectTaskId, string ProjectLeaderId) : IRequest { }
