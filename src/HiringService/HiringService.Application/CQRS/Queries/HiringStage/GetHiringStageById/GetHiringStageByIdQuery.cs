using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.HiringStageQueries;

public sealed record GetHiringStageByIdQuery(int Id) : IRequest<HiringStage> { }
