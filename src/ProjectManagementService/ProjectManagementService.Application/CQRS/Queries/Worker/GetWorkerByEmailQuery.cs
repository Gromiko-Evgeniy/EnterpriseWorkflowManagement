using MediatR;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.WorkerQueries;

public sealed record GetWorkerByEmailQuery(string Email) : IRequest<Worker> { }
