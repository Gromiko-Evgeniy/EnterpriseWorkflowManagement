using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.StageQueries;

public sealed record GetHiringStagesQuery : IRequest<List<HiringStage>> { }
