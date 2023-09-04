using AutoMapper;
using HiringService.Application.Abstractions.RepositoryAbstractions;
using HiringService.Application.Exceptions.Worker;
using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.WorkerCommands;

public class AddWorkerHandler : IRequestHandler<AddWorkerCommand, int>
{
    private readonly IWorkerRepository _workerRepository;
    private readonly IMapper _mapper;

    public AddWorkerHandler(IWorkerRepository workerRepository, IMapper mapper)
    {
        _mapper = mapper;
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

        return newWorker.Id;
    }
}
