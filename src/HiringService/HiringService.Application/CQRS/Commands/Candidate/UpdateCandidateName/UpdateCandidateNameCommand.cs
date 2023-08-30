using MediatR;

namespace HiringService.Application.CQRS.CandidateCommands;

public sealed record UpdateCandidateNameCommand(int Id, string Name) : IRequest { }
