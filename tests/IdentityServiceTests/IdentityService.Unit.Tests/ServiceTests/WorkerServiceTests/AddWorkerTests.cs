using IdentityService.Application.DTOs;
using IdentityService.Application.DTOs.WorkerDTOs;
using IdentityService.Application.Exceptions.Worker;
using IdentityService.Application.KafkaAbstractions;

namespace IdentityService.Unit.Tests.ServiceTests.WorkerServiceTests;

public class AddWorkerTests
{
    private readonly Mock<IWorkerRepository> _workerRepositoryMock;
    private readonly Mock<IKafkaProducer> _kafkaProducerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly WorkerService _workerService;

    public AddWorkerTests()
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
    public async Task AddAsync_WhenWorkerDoesNotExist_AddsNewWorkerAndReturnsLogInData()
    {
        // Arrange
        var workerDTO = new AddWorkerDTO { Email = "test@example.com" };
        var newWorker = new Worker { Email = workerDTO.Email };
        var logInData = new LogInData { Email = workerDTO.Email };

        _workerRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Worker, bool>>>()))
            .ReturnsAsync((Worker)null!);
        _mapperMock.Setup(mapper => mapper.Map<Worker>(workerDTO)).Returns(newWorker);
        _mapperMock.Setup(mapper => mapper.Map<LogInData>(newWorker)).Returns(logInData);

        // Act
        var result = await _workerService.AddAsync(workerDTO);

        // Assert
        result.Should().BeEquivalentTo(logInData);
        _workerRepositoryMock.Verify(repo => repo.Add(newWorker), Times.Once);
        _workerRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task AddAsync_WhenWorkerExists_ThrowsWorkerAlreadyExistsException()
    {
        // Arrange
        var workerDTO = new AddWorkerDTO { Email = "test@example.com" };
        var existingWorker = new Worker { Email = workerDTO.Email };

        _workerRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Worker, bool>>>()))
            .ReturnsAsync(existingWorker);

        // Act
        Func<Task> act = async () => await _workerService.AddAsync(workerDTO);

        // Assert
        await act.Should().ThrowAsync<WorkerAlreadyExistsException>();
        _workerRepositoryMock.Verify(repo => repo.Add(It.IsAny<Worker>()), Times.Never);
        _workerRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
    }
}
