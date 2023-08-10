using MediatR;

namespace ProjectManagementService.Application.CQRS.ProjectTaskCommands;

public sealed record FinishWorkingOnTaskCommand(string WorkerId) : IRequest { }