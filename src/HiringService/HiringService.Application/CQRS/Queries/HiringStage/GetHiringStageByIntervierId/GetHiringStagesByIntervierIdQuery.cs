using HiringService.Application.DTOs.HiringStageDTOs;
using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.HiringStageQueries;

public sealed record GetHiringStagesByIntervierIdQuery(int IntervierId) : IRequest<List<HiringStageShortInfoDTO>> { }
