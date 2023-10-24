using HiringService.Application.Cache;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.Exceptions.Customer;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.CustomerCommands;

public class RemoveCustomerHandler : IRequestHandler<RemoveCustomerCommand>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly IDistributedCache _cache;

    public RemoveCustomerHandler(
        ICustomerRepository customerRepository,
        IProjectRepository projectRepository,
        IProjectTaskRepository projectTaskRepository,
        IDistributedCache cache)
    {
        _customerRepository = customerRepository;
        _projectRepository = projectRepository;
        _projectTaskRepository = projectTaskRepository;
        _cache = cache;
    }

    public async Task<Unit> Handle(RemoveCustomerCommand request, CancellationToken cancellationToken)
    {
        var emailKey = RedisKeysPrefixes.CustomerPrefix + request.Email;
        var customer = await _cache.GetRecordAsync<Customer>(emailKey);

        if (customer is null)
        {
            customer = await _customerRepository
                .GetFirstAsync(customer => customer.Email == request.Email);
        }
        
        if (customer is null) throw new NoCustomerWithSuchIdException();

        var projects = await _projectRepository.GetFilteredAsync(p => p.CustomerId == customer.Id);
        var tasks = await _projectTaskRepository.GetAllAsync();

        foreach (var project in projects)
        {
            await _projectRepository.CancelAsync(project.Id);

            var projectTasks = tasks.Where(t => t.ProjectId == project.Id);

            foreach (var task in projectTasks)
            {
                await _projectTaskRepository.CancelAsync(task.Id);
            }
        }

        await _customerRepository.RemoveAsync(customer.Id);

        await _cache.RemoveRecordAsync(emailKey);

        return Unit.Value;
    }
}
