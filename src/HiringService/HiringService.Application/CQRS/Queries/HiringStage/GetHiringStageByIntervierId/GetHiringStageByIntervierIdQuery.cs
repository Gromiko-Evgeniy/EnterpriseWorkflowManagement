using HiringService.Application.DTOs.HiringStageDTOs;
using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.HiringStageQueries;

public sealed record GetHiringStageByIntervierIdQuery(int IntervierId) : IRequest<List<HiringStageShortInfoDTO>> { }
