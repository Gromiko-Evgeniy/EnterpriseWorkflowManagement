using MediatR;

namespace HiringService.Application.CQRS.WorkerCommands;

public sealed record RemoveWorkerCommand(string Email) : IRequest { }
