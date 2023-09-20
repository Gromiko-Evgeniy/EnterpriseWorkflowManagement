using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.WorkerQueries;

public sealed record GetWorkerByEmailQuery(string Email) : IRequest<Worker> { }
