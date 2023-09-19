using AutoMapper;
using HiringService.Application.Cache;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.Exceptions.Worker;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.WorkerCommands;

public class RemoveWorkerHandler : IRequestHandler<RemoveWorkerCommand>
{
    private readonly IWorkerRepository _workerRepository;
    private readonly IDistributedCache _cache;

    public RemoveWorkerHandler(IWorkerRepository workerRepository,
        IDistributedCache cache)
    {
        _workerRepository = workerRepository;
        _cache = cache;
    }

    public async Task<Unit> Handle(RemoveWorkerCommand request, CancellationToken cancellationToken)
    {
        var emailKey = RedisKeysPrefixes.WorkerPrefix + request.Email;
        var worker = await _cache.GetRecordAsync<Worker>(emailKey);

        if (worker is null)
        {
            worker = await _workerRepository.
                GetFirstAsync(worker => worker.Email == request.Email);
        }

        if (worker is null) throw new NoWorkerWithSuchEmailException();

        await _workerRepository.RemoveAsync(worker.Id);

        await _cache.RemoveAsync(emailKey);

        return Unit.Value;
    }
}
