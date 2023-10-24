using IdentityService.Application.Exceptions;
using IdentityService.Application.Exceptions.Worker;
using IdentityService.Application.KafkaAbstractions;

namespace IdentityService.Unit.Tests.ServiceTests.WorkerServiceTests;

public class WorkerQuitsTests
{
    private readonly Mock<IWorkerRepository> _workerRepositoryMock;
    private readonly Mock<IKafkaProducer> _kafkaProducerMock;
    private readonly WorkerService _workerService;

    public WorkerQuitsTests()
    {
        _workerRepositoryMock = new Mock<IWorkerRepository>();
        _kafkaProducerMock = new Mock<IKafkaProducer>();
        _workerService = new WorkerService(
            _workerRepositoryMock.Object,
            null!,
            _kafkaProducerMock.Object);
    }

    [Fact]
    public async Task QuitAsync_WhenWorkerExists_RemovesWorker()
    {
        // Arrange
        var email = "test@example.com";
        var password = "password123";

        var worker = new Worker { Email = email, Password = password };
        var workers = new List<Worker>() { worker, new Worker() };

        _workerRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Worker, bool>>>()))
            .ReturnsAsync(worker);

        _workerRepositoryMock
           .Setup(repo => repo.Remove(worker))
           .Callback(() => workers.Remove(worker));

        // Act
        await _workerService.QuitAsync(email, password);

        // Assert
        _workerRepositoryMock.Verify(repo => repo.Remove(worker), Times.Once);
        workers.Count.Should().Be(1);
        workers.Contains(worker).Should().Be(false);
    }

    [Fact]
    public async Task QuitAsync_WhenWorkerDoesNotExist_ThrowsNoWorkerWithSuchEmailException()
    {
        // Arrange
        var email = "test@example.com";
        var password = "password123";

        _workerRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Worker, bool>>>()))
            .ReturnsAsync((Worker)null!);

        // Act
        Func<Task> act = async () => await _workerService.QuitAsync(email, password);

        // Assert
        await act.Should().ThrowAsync<NoWorkerWithSuchEmailException>();
        _workerRepositoryMock.Verify(repo => repo.Remove(It.IsAny<Worker>()), Times.Never);
        _workerRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task QuitAsync_WhenPasswordIsIncorrect_ThrowsIncorrectPasswordException()
    {
        // Arrange
        var email = "test@example.com";
        var password = "password123";
        var worker = new Worker { Email = email, Password = "wrongpassword" };

        _workerRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Worker, bool>>>()))
            .ReturnsAsync(worker);

        // Act
        Func<Task> act = async () => await _workerService.QuitAsync(email, password);

        // Assert
        await act.Should().ThrowAsync<IncorrectPasswordException>();
        _workerRepositoryMock.Verify(repo => repo.Remove(It.IsAny<Worker>()), Times.Never);
        _workerRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
    }
}
