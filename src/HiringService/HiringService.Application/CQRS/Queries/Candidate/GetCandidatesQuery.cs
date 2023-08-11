using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.CandidateQueries;

public sealed record GetCandidatesQuery : IRequest<List<Candidate>> { }

