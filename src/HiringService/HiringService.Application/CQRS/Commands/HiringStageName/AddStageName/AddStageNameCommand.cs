using HiringService.Application.DTOs.StageNameDTOs;
using MediatR;

namespace HiringService.Application.CQRS.StageNameCommands;

public sealed record AddStageNameCommand(AddStageNameDTO StageNameDTO) : IRequest<int> { }
