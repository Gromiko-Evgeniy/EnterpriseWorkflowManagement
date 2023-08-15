using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.CandidateQueries;

public sealed record GetCandidateByEmailQuery(string Email) : IRequest<Candidate> { }