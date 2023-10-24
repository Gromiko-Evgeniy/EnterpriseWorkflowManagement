using IdentityService.Application.Exceptions.Worker;
using IdentityService.Application.KafkaAbstractions;

namespace IdentityService.Unit.Tests.ServiceTests.WorkerServiceTests;

public class UpdateWorkerNameTests
{
    private readonly Mock<IWorkerRepository> _workerRepositoryMock;
    private readonly Mock<IKafkaProducer> _kafkaProducerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly WorkerService _workerService;

    public UpdateWorkerNameTests()
    {
        _workerRepositoryMock = new Mock<IWorkerRepository>();
        _kafkaProducerMock = new Mock<IKafkaProducer>();
        _mapperMock = new Mock<IMapper>();
        _workerService = new WorkerService(
            _workerRepositoryMock.Object,
            _mapperMock.Object,
            _kafkaProducerMock.Object);
    }

    [Fact]
    public async Task UpdateNameAsync_WhenWorkerExists_UpdatesWorkerName()
    {
        // Arrange
        var email = "test@example.com";
        var name = "John Doe";
        var worker = new Worker { Email = email };

        _workerRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Worker, bool>>>()))
            .ReturnsAsync(worker);

        // Act
        await _workerService.UpdateNameAsync(email, name);

        // Assert
        worker.Name.Should().Be(name);
        _workerRepositoryMock.Verify(repo => repo.Update(worker), Times.Once);
        //_workerRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateNameAsync_WhenWorkerDoesNotExist_ThrowsNoWorkerWithSuchEmailException()
    {
        // Arrange
        var email = "test@example.com";
        var name = "John Doe";

        _workerRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Worker, bool>>>()))
            .ReturnsAsync((Worker)null!);

        // Act
        Func<Task> act = async () => await _workerService.UpdateNameAsync(email, name);

        // Assert
        await act.Should().ThrowAsync<NoWorkerWithSuchEmailException>();
        _workerRepositoryMock.Verify(repo => repo.Update(It.IsAny<Worker>()), Times.Never);
        _workerRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
    }
}
