using AutoMapper;
using HiringService.Application.Abstractions.RepositoryAbstractions;
using HiringService.Application.Cache;
using HiringService.Application.Exceptions.Worker;
using HiringService.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace HiringService.Application.CQRS.WorkerCommands;

public class AddWorkerHandler : IRequestHandler<AddWorkerCommand, int>
{
    private readonly IWorkerRepository _workerRepository;
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;

    public AddWorkerHandler(IWorkerRepository workerRepository,
        IMapper mapper, IDistributedCache cache)
    {
        _mapper = mapper;
        _cache = cache;
        _workerRepository = workerRepository;
    }

    public async Task<int> Handle(AddWorkerCommand request, CancellationToken cancellationToken)
    {
        var workerDTO = request.NameEmailDTO;

        var oldWorker = await _workerRepository.
            GetFirstAsync(worker => worker.Email == workerDTO.Email);

        if (oldWorker is not null) throw new WorkerAlreadyExistsException();

        var newWorker = _mapper.Map<Worker>(workerDTO);

        newWorker = _workerRepository.Add(newWorker);
        await _workerRepository.SaveChangesAsync();

        var emailKey = "Worker_" + newWorker.Email;

        await _cache.SetRecordAsync(emailKey, newWorker);

        return newWorker.Id;
    }
}
