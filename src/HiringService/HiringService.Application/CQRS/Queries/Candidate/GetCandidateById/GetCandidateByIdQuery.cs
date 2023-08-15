using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.CandidateQueries;

public sealed record GetCandidateByIdQuery(int Id) : IRequest<Candidate> { }