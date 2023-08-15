using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.StageNameQueries;

public sealed record GetHiringStageNameByIdQuery(int Id) : IRequest<HiringStageName> { }