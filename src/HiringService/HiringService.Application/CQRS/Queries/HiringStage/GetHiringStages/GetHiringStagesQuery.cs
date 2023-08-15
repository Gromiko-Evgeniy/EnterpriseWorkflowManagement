using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.HiringStageQueries;

public sealed record GetHiringStagesQuery : IRequest<List<HiringStage>> { }
