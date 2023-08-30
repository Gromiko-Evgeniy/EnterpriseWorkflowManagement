using HiringService.Application.DTOs.CandidateDTOs;
using MediatR;

namespace HiringService.Application.CQRS.CandidateQueries;

public sealed record GetCandidatesQuery : IRequest<List<CandidateShortInfoDTO>> { }

