using HiringService.Application.DTOs.HiringStageDTOs;
using MediatR;

namespace HiringService.Application.CQRS.HiringStageQueries;

public sealed record GetHiringStagesQuery : IRequest<List<HiringStageShortInfoDTO>> { }
