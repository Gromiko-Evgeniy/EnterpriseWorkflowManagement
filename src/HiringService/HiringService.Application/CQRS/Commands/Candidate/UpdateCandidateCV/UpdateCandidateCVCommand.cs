using MediatR;

namespace HiringService.Application.CQRS.CandidateCommands;

public sealed record UpdateCandidateCVCommand(int Id, string CV) : IRequest { }
