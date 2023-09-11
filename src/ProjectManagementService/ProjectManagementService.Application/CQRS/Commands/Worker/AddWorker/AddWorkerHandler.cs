using AutoMapper;
using HiringService.Application.Cache;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.Exceptions.Worker;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.WorkerCommands;

public class AddWorkerHandler : IRequestHandler<AddWorkerCommand, string>
{
    private readonly IWorkerRepository _workerRepository;
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;

    public AddWorkerHandler(IWorkerRepository workerRepository,
        IDistributedCache cache, IMapper mapper)
    {
        _workerRepository = workerRepository;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<string> Handle(AddWorkerCommand request, CancellationToken cancellationToken)
    {
        var workerDTO = request.NameEmailDTO;

        var oldWorker = await _workerRepository.
            GetFirstAsync(worker => worker.Email == workerDTO.Email);

        if (oldWorker is not null) throw new WorkerAlreadyExistsException();

        var newWorker = _mapper.Map<Worker>(workerDTO);

        string id = await _workerRepository.AddAsync(newWorker);

        var emailKey = "Worker_" + newWorker.Email;
        await _cache.SetRecordAsync(emailKey, newWorker);

        return id;
    }
}
