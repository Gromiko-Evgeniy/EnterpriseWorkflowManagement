using MediatR;

namespace HiringService.Application.CQRS.StageNameCommands;

public sealed record RemoveStageNameCommand(int Id) : IRequest { }
