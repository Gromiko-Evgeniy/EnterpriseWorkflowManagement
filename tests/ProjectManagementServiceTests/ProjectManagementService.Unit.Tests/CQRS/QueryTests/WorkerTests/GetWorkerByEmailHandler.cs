using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.CQRS.WorkerQueries;
using ProjectManagementService.Application.Exceptions.Worker;

namespace ProjectManagementService.Unit.Tests.CQRS.QueryTests.WorkerTests;

public class GetWorkerByEmailHandlerTests
{
    private readonly Mock<IWorkerRepository> _workerRepositoryMock;
    private readonly GetWorkerByEmailHandler _handler;

    public GetWorkerByEmailHandlerTests()
    {
        _workerRepositoryMock = new Mock<IWorkerRepository>();
        _handler = new GetWorkerByEmailHandler(_workerRepositoryMock.Object, null!);
    }

    [Fact]
    public async Task Handle_WithExistingWorkerEmail_ReturnsWorker()
    {
        // Arrange
        var worker = new Worker { Email = "test@example.com" };
        var query = new GetWorkerByEmailQuery(worker.Email);

        _workerRepositoryMock.Setup(r => r.GetFirstAsync(w => w.Email == query.Email))
            .Callback(() => { Console.WriteLine(worker.Email == query.Email); })
            .ReturnsAsync(worker);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().Be(worker);
    }

    [Fact]
    public async Task Handle_WithNonExistingWorkerEmail_ThrowsNoWorkerWithSuchEmailException()
    {
        // Arrange
        var query = new GetWorkerByEmailQuery("test@example.com");

        _workerRepositoryMock.Setup(r => r.GetFirstAsync(w => w.Email == query.Email))
            .ReturnsAsync((Worker)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NoWorkerWithSuchEmailException>(() => _handler.Handle(query, CancellationToken.None));
    }
}
