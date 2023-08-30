using MediatR;

namespace HiringService.Application.CQRS.StageNameCommands;

public sealed record AddStageNameCommand(string Name) : IRequest<int> { }
