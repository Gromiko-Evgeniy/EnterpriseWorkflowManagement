using IdentityService.Application.Exceptions.Worker;

namespace IdentityService.Unit.Tests.ServiceTests.WorkerServiceTests;

public class UpdateWorkerLastTimeVisitedTests
{
    private readonly Mock<IWorkerRepository> _workerRepositoryMock;
    private readonly WorkerService _workerService;

    public UpdateWorkerLastTimeVisitedTests()
    {
        _workerRepositoryMock = new Mock<IWorkerRepository>();
        _workerService = new WorkerService(_workerRepositoryMock.Object, null!, null!);
    }

    [Fact]
    public async Task UpdateLastTimeVisitedAsync_WhenWorkerExists_UpdatesLastTimeVisited()
    {
        // Arrange
        var email = "test@example.com";
        var worker = new Worker { Email = email };
        var currentTime = DateTime.Now;

        _workerRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Worker, bool>>>()))
            .ReturnsAsync(worker);

        // Act
        await _workerService.UpdateLastTimeVisitedAsync(email);

        // Assert
        worker.LastTimeVisited.Should().BeAfter(currentTime);
        _workerRepositoryMock.Verify(repo => repo.Update(worker), Times.Once);
        _workerRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateLastTimeVisitedAsync_WhenWorkerDoesNotExist_ThrowsNoWorkerWithSuchEmailException()
    {
        // Arrange
        var email = "test@example.com";

        _workerRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Worker, bool>>>()))
            .ReturnsAsync((Worker)null);

        // Act
        Func<Task> act = async () => await _workerService.UpdateLastTimeVisitedAsync(email);

        // Assert
        await act.Should().ThrowAsync<NoWorkerWithSuchEmailException>();
        _workerRepositoryMock.Verify(repo => repo.Update(It.IsAny<Worker>()), Times.Never);
        _workerRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
    }
}
