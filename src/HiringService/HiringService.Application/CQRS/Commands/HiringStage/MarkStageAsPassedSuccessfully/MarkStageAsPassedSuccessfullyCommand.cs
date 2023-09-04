using MediatR;

namespace HiringService.Application.CQRS.HiringStageCommands;

public sealed record MarkStageAsPassedSuccessfullyCommand(int IntervierId, int StageId) : IRequest<string> { }
