using HiringService.Application.DTOs.StageNameDTOs;
using MediatR;

namespace HiringService.Application.CQRS.StageNameQueries;

public sealed record GetHiringStageNamesQuery : IRequest<List<GetStageNameDTO>> { }
