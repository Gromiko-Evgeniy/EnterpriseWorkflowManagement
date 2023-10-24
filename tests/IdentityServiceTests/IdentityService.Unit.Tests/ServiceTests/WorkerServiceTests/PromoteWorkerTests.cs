using IdentityService.Application.Exceptions.Worker;

namespace IdentityService.Unit.Tests.ServiceTests.WorkerServiceTests;

public class PromoteWorkerTests
{
    private readonly Mock<IWorkerRepository> _workerRepositoryMock;
    private readonly WorkerService _workerService;

    public PromoteWorkerTests()
    {
        _workerRepositoryMock = new Mock<IWorkerRepository>();
        _workerService = new WorkerService(_workerRepositoryMock.Object, null!, null!);
    }

    [Fact]
    public async Task PromoteAsync_WhenWorkerPositionIsNotDepartmentHead_PromotesWorker()
    {
        // Arrange
        var email = "test@example.com";
        var worker = new Worker { Position = Position.Worker };

        _workerRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Worker, bool>>>()))
            .ReturnsAsync(worker);

        // Act
        await _workerService.PromoteAsync(email);

        // Assert
        worker.Position.Should().Be(Position.ProjectLeader);
        _workerRepositoryMock.Verify(repo => repo.Update(worker), Times.Once);
        _workerRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task PromoteAsync_WhenWorkerPositionIsDepartmentHead_ThrowsCanNotPromoteWorkerException()
    {
        // Arrange
        var email = "test@example.com";
        var worker = new Worker { Position = Position.DepartmentHead };

        _workerRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Worker, bool>>>()))
            .ReturnsAsync(worker);

        // Act
        Func<Task> act = async () => await _workerService.PromoteAsync(email);

        // Assert
        await act.Should().ThrowAsync<CanNotPromoteWorkerException>();
        _workerRepositoryMock.Verify(repo => repo.Update(It.IsAny<Worker>()), Times.Never);
        _workerRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
    }
}
