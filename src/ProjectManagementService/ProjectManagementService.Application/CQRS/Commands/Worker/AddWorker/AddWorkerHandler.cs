using AutoMapper;
using MediatR;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.Exceptions.Worker;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.WorkerCommands;

public class AddWorkerHandler : IRequestHandler<AddWorkerCommand, string>
{
    private readonly IWorkerRepository _workerRepository;
    private readonly IMapper _mapper;

    public AddWorkerHandler(IWorkerRepository workerRepository, IMapper mapper)
    {
        _mapper = mapper;
        _workerRepository = workerRepository;
    }

    public async Task<string> Handle(AddWorkerCommand request, CancellationToken cancellationToken)
    {
        var workerDTO = request.NameEmailDTO;

        var oldWorker = await _workerRepository.
            GetFirstAsync(worker => worker.Email == workerDTO.Email);

        if (oldWorker is not null) throw new WorkerAlreadyExistsException();

        var newWorker = _mapper.Map<Worker>(workerDTO);

        string id = await _workerRepository.AddAsync(newWorker);

        return id;
    }
}
