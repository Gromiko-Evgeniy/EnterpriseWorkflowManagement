using MediatR;

namespace HiringService.Application.CQRS.HiringStageCommands;

public sealed record AddHiringStageIntervierCommand(int IntervierId, int StageId) : IRequest { }
