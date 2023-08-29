using HiringService.Application.DTOs.CandidateDTOs;
using MediatR;

namespace HiringService.Application.CQRS.CandidateQueries;

public sealed record GetCandidateByIdQuery(int Id) : IRequest<CandidateMainInfoDTO> { }
