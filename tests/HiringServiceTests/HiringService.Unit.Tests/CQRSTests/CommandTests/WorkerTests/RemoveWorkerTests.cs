using HiringService.Application.CQRS.WorkerCommands;
using HiringService.Application.Exceptions.Worker;

namespace HiringService.Unit.Tests.CQRSTests.CommandTests.WorkerTests;

public class RemoveWorkerTests
{
    private readonly Mock<IWorkerRepository> _workerRepositoryMock;
    private readonly RemoveWorkerHandler _handler;

    public RemoveWorkerTests()
    {
        _workerRepositoryMock = new Mock<IWorkerRepository>();
        _handler = new RemoveWorkerHandler(_workerRepositoryMock.Object, null!);
    }

    [Fact]
    public async Task Handle_WithExistingWorker_RemovesWorkerFromRepository()
    {
        // Arrange
        var worker = new Worker { Email = "john@example.com" };
        var command = new RemoveWorkerCommand(worker.Email);

        _workerRepositoryMock.Setup(r => r.GetByEmailAsync(worker.Email))
            .ReturnsAsync(worker);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _workerRepositoryMock.Verify(r => r.Remove(worker), Times.Once);
        _workerRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistingWorker_ThrowsNoWorkerWithSuchEmailException()
    {
        // Arrange
        var command = new RemoveWorkerCommand("john@example.com");

        _workerRepositoryMock.Setup(r => r.GetByEmailAsync(command.Email))
            .ReturnsAsync((Worker)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NoWorkerWithSuchEmailException>(() => _handler.Handle(command, CancellationToken.None));

        // Assert
        _workerRepositoryMock.Verify(r => r.Remove(It.IsAny<Worker>()), Times.Never);
        _workerRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}

