using HiringService.Application.CQRS.WorkerQueries;
using HiringService.Application.Exceptions.Worker;

namespace HiringService.Unit.Tests.CQRSTests.QueryTests.WorkerTests;

public class GetWorkerByEmailTests
{
    private readonly Mock<IWorkerRepository> _workerRepositoryMock;
    private readonly GetWorkerByEmailHandler _handler;

    public GetWorkerByEmailTests()
    {
        _workerRepositoryMock = new Mock<IWorkerRepository>();
        _handler = new GetWorkerByEmailHandler(_workerRepositoryMock.Object, null!);
    }

    [Fact]
    public async Task Handle_ValidEmail_ReturnsWorker()
    {
        // Arrange
        var email = "test@example.com";
        var worker = new Worker { Email = email, Name = "John Doe" };

        _workerRepositoryMock.Setup(repo => repo.GetByEmailAsync(email))
            .ReturnsAsync(worker);

        // Act
        var result = await _handler.Handle(new GetWorkerByEmailQuery(email), CancellationToken.None);

        // Assert
        Assert.Equal(worker, result);
        _workerRepositoryMock.Verify(repo => repo.GetByEmailAsync(email), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidEmail_ThrowsNoCandidateWithSuchEmailException()
    {
        // Arrange
        var email = "test@example.com";

        _workerRepositoryMock.Setup(repo => repo.GetByEmailAsync(email))
            .ReturnsAsync((Worker)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NoWorkerWithSuchEmailException>(() => _handler.Handle(new GetWorkerByEmailQuery(email), CancellationToken.None));
        _workerRepositoryMock.Verify(repo => repo.GetByEmailAsync(email), Times.Once);
    }
}
