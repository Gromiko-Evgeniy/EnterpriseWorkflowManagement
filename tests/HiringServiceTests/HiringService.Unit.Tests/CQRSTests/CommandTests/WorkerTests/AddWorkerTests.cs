using HiringService.Application.CQRS.WorkerCommands;
using HiringService.Application.Exceptions.Worker;
using ProjectManagementService.Application.DTOs;

namespace HiringService.Unit.Tests.CQRSTests.CommandTests.WorkerTests;

public class AddWorkerTests
{
    private readonly Mock<IWorkerRepository> _workerRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AddWorkerHandler _handler;

    public AddWorkerTests()
    {
        _workerRepositoryMock = new Mock<IWorkerRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new AddWorkerHandler(_workerRepositoryMock.Object, _mapperMock.Object, null);
    }

    [Fact]
    public async Task Handle_WithNewWorker_ReturnsWorkerId()
    {
        // Arrange
        var command = new AddWorkerCommand(new NameEmailDTO { Name = "John", Email = "john@example.com" });
        var worker = new Worker { Id = 1, Name = "John", Email = "john@example.com" };

        _workerRepositoryMock.Setup(r => r.GetByEmailAsync(worker.Email))
            .ReturnsAsync((Worker)null!);

        _mapperMock.Setup(m => m.Map<Worker>(command.NameEmailDTO))
            .Returns(worker);

        _workerRepositoryMock.Setup(r => r.Add(worker))
            .Returns(worker);

        _workerRepositoryMock.Setup(r => r.SaveChangesAsync());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(worker.Id, result);
        _workerRepositoryMock.Verify(r => r.GetByEmailAsync(worker.Email), Times.Once);
        _mapperMock.Verify(m => m.Map<Worker>(command.NameEmailDTO), Times.Once);
        _workerRepositoryMock.Verify(r => r.Add(worker), Times.Once);
        _workerRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);

    }

    [Fact]
    public async Task Handle_WithExistingWorker_ThrowsWorkerAlreadyExistsException()
    {
        // Arrange
        var command = new AddWorkerCommand(new NameEmailDTO { Name = "John", Email = "john@example.com" });
        var existingWorker = new Worker { Id = 1, Name = "John", Email = "john@example.com" };

        _workerRepositoryMock.Setup(r => r.GetByEmailAsync(existingWorker.Email))
            .ReturnsAsync(existingWorker);

        // Act & Assert
        await Assert.ThrowsAsync<WorkerAlreadyExistsException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
