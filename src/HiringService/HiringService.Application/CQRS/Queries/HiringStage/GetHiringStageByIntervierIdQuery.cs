using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.StageQueries;

public sealed record GetHiringStageByIntervierIdQuery(int IntervierId) : IRequest<List<HiringStage>> { }

