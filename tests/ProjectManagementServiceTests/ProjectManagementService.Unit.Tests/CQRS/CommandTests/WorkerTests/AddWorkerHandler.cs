using ProjectManagementService.Application.CQRS.WorkerCommands;
using ProjectManagementService.Application.DTOs;
using ProjectManagementService.Application.Exceptions.Worker;

namespace ProjectManagementService.Unit.Tests.CQRS.CommandTests.WorkerTests;

public class AddWorkerHandlerTests
{
    private readonly Mock<IWorkerRepository> _workerRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AddWorkerHandler _handler;

    public AddWorkerHandlerTests()
    {
        _workerRepositoryMock = new Mock<IWorkerRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new AddWorkerHandler(_workerRepositoryMock.Object, null!, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidData_ReturnsWorkerId()
    {
        // Arrange
        var command = new AddWorkerCommand(new NameEmailDTO());

        var workerDTO = command.NameEmailDTO;
        var worker = new Worker { Id = "1" };

        _workerRepositoryMock
            .Setup(r => r.GetFirstAsync(w => w.Email == workerDTO.Email))
            .ReturnsAsync((Worker)null!);

        _mapperMock
            .Setup(m => m.Map<Worker>(workerDTO))
            .Returns(worker);

        _workerRepositoryMock
            .Setup(r => r.AddOneAsync(worker))
            .ReturnsAsync(worker.Id);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(worker.Id);
    }

    [Fact]
    public async Task Handle_WithExistingWorker_ThrowsWorkerAlreadyExistsException()
    {
        // Arrange
        var command = new AddWorkerCommand(new NameEmailDTO());
        var workerDTO = command.NameEmailDTO;
        var existingWorker = new Worker { Id = "1" };

        _workerRepositoryMock.Setup(r => r.GetFirstAsync(w => w.Email == workerDTO.Email))
            .ReturnsAsync(existingWorker);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<WorkerAlreadyExistsException>();
    }
}
