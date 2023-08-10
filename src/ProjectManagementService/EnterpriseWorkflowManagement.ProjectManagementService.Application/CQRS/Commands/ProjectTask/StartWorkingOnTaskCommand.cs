using MediatR;

namespace ProjectManagementService.Application.CQRS.ProjectTaskCommands;

public sealed record StartWorkingOnTaskCommand(string WorkerId) : IRequest { }
