using IdentityService.Application.Exceptions.Worker;

namespace IdentityService.Unit.Tests.ServiceTests.WorkerServiceTests;

public class DemoteWorkerTests
{
    private readonly Mock<IWorkerRepository> _workerRepositoryMock;
    private readonly WorkerService _workerService;

    public DemoteWorkerTests()
    {
        _workerRepositoryMock = new Mock<IWorkerRepository>();
        _workerService = new WorkerService(_workerRepositoryMock.Object, null!, null!);
    }

    [Fact]
    public async Task DemoteAsync_WhenWorkerPositionIsNotWorker_DemotesWorker()
    {
        // Arrange
        var email = "test@example.com";
        var worker = new Worker { Position = Position.ProjectLeader};

        _workerRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Worker, bool>>>()))
            .ReturnsAsync(worker);

        // Act
        await _workerService.DemoteAsync(email);

        // Assert
        worker.Position.Should().Be(Position.Worker);
        _workerRepositoryMock.Verify(repo => repo.Update(worker), Times.Once);
        _workerRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DemoteAsync_WhenWorkerPositionIsWorker_ThrowsCanNotDemoteWorkerException()
    {
        // Arrange
        var email = "test@example.com";
        var worker = new Worker { Position = Position.Worker };

        _workerRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Worker, bool>>>()))
            .ReturnsAsync(worker);

        // Act
        Func<Task> act = async () => await _workerService.DemoteAsync(email);

        // Assert
        await act.Should().ThrowAsync<CanNotDemoteWorkerException>();
        _workerRepositoryMock.Verify(repo => repo.Update(It.IsAny<Worker>()), Times.Never);
        _workerRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
    }
}

