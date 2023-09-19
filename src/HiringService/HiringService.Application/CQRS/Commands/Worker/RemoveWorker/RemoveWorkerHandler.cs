using AutoMapper;
using HiringService.Application.Abstractions.RepositoryAbstractions;
using HiringService.Application.Cache;
using HiringService.Application.Exceptions.Worker;
using HiringService.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace HiringService.Application.CQRS.WorkerCommands;

public class RemoveWorkerHandler : IRequestHandler<RemoveWorkerCommand>
{
    private readonly IWorkerRepository _workerRepository;
    private readonly IDistributedCache _cache;

    public RemoveWorkerHandler(IWorkerRepository workerRepository, IDistributedCache cache)
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

        _workerRepository.Remove(worker);
        await _workerRepository.SaveChangesAsync();

        await _cache.RemoveAsync(emailKey);

        return Unit.Value;
    }
}
