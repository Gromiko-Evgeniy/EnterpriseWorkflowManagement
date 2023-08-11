using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.StageNameQueries;

public sealed record GetHiringStageNameByNameQuery(string Name) : IRequest<HiringStageName> { }
