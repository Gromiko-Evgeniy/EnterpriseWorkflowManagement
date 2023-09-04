using MediatR;

namespace ProjectManagementService.Application.CQRS.WorkerCommands;

public sealed record RemoveWorkerCommand(string Email) : IRequest { }
