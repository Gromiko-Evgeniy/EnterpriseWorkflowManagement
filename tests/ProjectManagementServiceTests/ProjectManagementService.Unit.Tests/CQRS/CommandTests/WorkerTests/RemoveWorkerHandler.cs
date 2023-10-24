using ProjectManagementService.Application.CQRS.WorkerCommands;
using ProjectManagementService.Application.Exceptions.Worker;
using System.Linq.Expressions;

namespace ProjectManagementService.Unit.Tests.CQRS.CommandTests.WorkerTests;

public class RemoveWorkerHandlerTests
{
    private readonly Mock<IWorkerRepository> _workerRepositoryMock;
    private readonly RemoveWorkerHandler _handler;

    public RemoveWorkerHandlerTests()
    {
        _workerRepositoryMock = new Mock<IWorkerRepository>();
        _handler = new RemoveWorkerHandler(_workerRepositoryMock.Object, null!);
    }

    [Fact]
    public async Task Handle_WithExistingWorker_RemovesWorker()
    {
        // Arrange
        var existingWorker = new Worker { Id = "1", Email = "test@example.com" };
        var command = new RemoveWorkerCommand(existingWorker.Email);

        _workerRepositoryMock.Setup(r => r.GetFirstAsync(It.IsAny<Expression<Func<Worker, bool>>>()))
            .ReturnsAsync(existingWorker);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _workerRepositoryMock.Verify(r => r.RemoveAsync(existingWorker.Id), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistingWorker_ThrowsNoWorkerWithSuchEmailException()
    {
        // Arrange
        var command = new RemoveWorkerCommand("test@example.com");

        _workerRepositoryMock.Setup(r => r.GetFirstAsync(It.IsAny<Expression<Func<Worker, bool>>>()))
            .ReturnsAsync((Worker)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NoWorkerWithSuchEmailException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
