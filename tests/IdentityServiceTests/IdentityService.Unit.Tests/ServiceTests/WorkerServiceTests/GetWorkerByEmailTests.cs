using IdentityService.Application.DTOs.WorkerDTOs;
using IdentityService.Application.Exceptions.Worker;

namespace IdentityService.Unit.Tests.ServiceTests.WorkerServiceTests;

public class GetWorkerByEmailTests
{
    private readonly Mock<IWorkerRepository> _workerRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly WorkerService _workerService;

    public GetWorkerByEmailTests()
    {
        _workerRepositoryMock = new Mock<IWorkerRepository>();
        _mapperMock = new Mock<IMapper>();
        _workerService = new WorkerService(_workerRepositoryMock.Object, _mapperMock.Object, null!);
    }

    [Fact]
    public async Task GetByEmailAsync_WhenWorkerExists_ReturnsWorkerDTO()
    {
        // Arrange
        var email = "test@example.com";
        var worker = new Worker();
        var workerDto = new GetWorkerDTO();

        _workerRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Worker, bool>>>()))
            .ReturnsAsync(worker);

        _mapperMock.Setup(mapper => mapper.Map<GetWorkerDTO>(worker))
            .Returns(workerDto);

        // Act
        var result = await _workerService.GetByEmailAsync(email);

        // Assert
        result.Should().BeEquivalentTo(workerDto);
    }

    [Fact]
    public async Task GetByEmailAsync_WhenWorkerDoesNotExist_ThrowsNoWorkerWithSuchEmailException()
    {
        // Arrange
        var email = "test@example.com";

        _workerRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Worker, bool>>>()))
            .ReturnsAsync((Worker)null!);

        // Act
        Func<Task> act = async () => await _workerService.GetByEmailAsync(email);

        // Assert
        await act.Should().ThrowAsync<NoWorkerWithSuchEmailException>();
    }
}
