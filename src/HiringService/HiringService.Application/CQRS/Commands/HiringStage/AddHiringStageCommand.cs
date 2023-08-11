using HiringService.Application.DTOs.HiringStageDTOs;
using MediatR;

namespace HiringService.Application.CQRS.HiringStageCommands;

public sealed record AddHiringStageCommand(AddHiringStageDTO StageDTO) : IRequest<int> { }
