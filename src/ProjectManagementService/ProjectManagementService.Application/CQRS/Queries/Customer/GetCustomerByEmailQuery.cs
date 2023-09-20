using MediatR;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.CustomerQueries;

public sealed record GetCustomerByEmailQuery(string Email) : IRequest<Customer> { }
