using IdentityService.Application.Exceptions.Worker;
using IdentityService.Application.KafkaAbstractions;

namespace IdentityService.Unit.Tests.ServiceTests.WorkerServiceTests;

public class DismissWorkerTests
{
    private readonly Mock<IWorkerRepository> _workerRepositoryMock;
    private readonly Mock<IKafkaProducer> _kafkaProducerMock;
    private readonly WorkerService _workerService;

    public DismissWorkerTests()
    {
        _workerRepositoryMock = new Mock<IWorkerRepository>();
        _kafkaProducerMock = new Mock<IKafkaProducer>();
        _workerService = new WorkerService(
            _workerRepositoryMock.Object,
            null!,
            _kafkaProducerMock.Object);
    }

    [Fact]
    public async Task DismissAsync_WhenWorkerExists_RemovesWorker()
    {
        // Arrange
        var email = "test@example.com";
        var worker = new Worker { Email = email };

        _workerRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Worker, bool>>>()))
            .ReturnsAsync(worker);

        // Act
        await _workerService.DismissAsync(email);

        // Assert
        _workerRepositoryMock.Verify(repo => repo.Remove(worker), Times.Once);
        _workerRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DismissAsync_WhenWorkerDoesNotExist_ThrowsNoWorkerWithSuchEmailException()
    {
        // Arrange
        var email = "test@example.com";

        _workerRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Worker, bool>>>()))
            .ReturnsAsync((Worker)null!);

        // Act
        Func<Task> act = async () => await _workerService.DismissAsync(email);

        // Assert
        await act.Should().ThrowAsync<NoWorkerWithSuchEmailException>();
        _workerRepositoryMock.Verify(repo => repo.Remove(It.IsAny<Worker>()), Times.Never);
        _workerRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
    }
}
