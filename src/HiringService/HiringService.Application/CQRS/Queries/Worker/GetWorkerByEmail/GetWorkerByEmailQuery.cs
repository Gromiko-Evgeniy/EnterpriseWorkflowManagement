using HiringService.Application.DTOs.CandidateDTOs;
using MediatR;

namespace HiringService.Application.CQRS.CandidateQueries;

public sealed record GetWorkerByEmailQuery(string Email) : IRequest<CandidateMainInfoDTO> { }
