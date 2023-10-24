using IdentityService.Application.DTOs;
using IdentityService.Application.DTOs.WorkerDTOs;
using IdentityService.Application.Exceptions;
using IdentityService.Application.Exceptions.Worker;

namespace IdentityService.Unit.Tests.ServiceTests.WorkerServiceTests;

public class GetWorkerByEmailAndPasswordTestsusing
{
    private readonly Mock<IWorkerRepository> _workerRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly WorkerService _workerService;

    public GetWorkerByEmailAndPasswordTestsusing()
    {
        _workerRepositoryMock = new Mock<IWorkerRepository>();
        _mapperMock = new Mock<IMapper>();
        _workerService = new WorkerService(_workerRepositoryMock.Object, _mapperMock.Object, null!);
    }

    [Fact]
    public async Task GetByEmailAndPasswordAsync_WhenValidCredentials_ReturnsWorkerDTO()
    {
        // Arrange
        var email = "test@example.com";
        var password = "password123";
        var logInData = new LogInData { Email = email, Password = password };
        var worker = new Worker { Email = email, Password = password, Name = "John Doe" };
        var workerDTO = new GetWorkerDTO { Email = email, Name = "John Doe" };

        _workerRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Worker, bool>>>()))
            .ReturnsAsync(worker);

        _mapperMock.Setup(mapper => mapper.Map<GetWorkerDTO>(worker))
            .Returns(workerDTO);

        // Act
        var result = await _workerService.GetByEmailAndPasswordAsync(logInData);

        // Assert
        result.Should().BeEquivalentTo(workerDTO);
    }

    [Fact]
    public async Task GetByEmailAndPasswordAsync_WhenInvalidCredentials_ThrowsIncorrectPasswordException()
    {
        // Arrange
        var email = "test@example.com";
        var password = "password123";
        var logInData = new LogInData { Email = email, Password = password };
        var worker = new Worker { Email = email, Password = "wrongpassword", Name = "John Doe" };

        _workerRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Worker, bool>>>()))
            .ReturnsAsync(worker);

        // Act
        Func<Task> act = async () => await _workerService.GetByEmailAndPasswordAsync(logInData);

        // Assert
        await act.Should().ThrowAsync<IncorrectPasswordException>();
    }

    [Fact]
    public async Task GetByEmailAndPasswordAsync_WhenWorkerDoesNotExist_ThrowsNoWorkerWithSuchEmailException()
    {
        // Arrange
        var email = "test@example.com";
        var password = "password123";
        var logInData = new LogInData { Email = email, Password = password };

        _workerRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Worker, bool>>>()))
            .ReturnsAsync((Worker)null!);

        // Act
        Func<Task> act = async () => await _workerService.GetByEmailAndPasswordAsync(logInData);

        // Assert
        await act.Should().ThrowAsync<NoWorkerWithSuchEmailException>();
    }
}

