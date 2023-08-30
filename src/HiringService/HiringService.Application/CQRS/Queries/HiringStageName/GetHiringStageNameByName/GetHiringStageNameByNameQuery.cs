using HiringService.Application.DTOs.StageNameDTOs;
using MediatR;

namespace HiringService.Application.CQRS.StageNameQueries;

public sealed record GetHiringStageNameByNameQuery(string Name) : IRequest<GetStageNameDTO> { }
