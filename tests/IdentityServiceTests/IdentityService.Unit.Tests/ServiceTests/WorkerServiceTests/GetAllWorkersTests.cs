using IdentityService.Application.DTOs.WorkerDTOs;

namespace IdentityService.Unit.Tests.ServiceTests.WorkerServiceTests;

public class GetAllWorkersTests
{
    private readonly Mock<IWorkerRepository> _workerRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly WorkerService _workerService;

    public GetAllWorkersTests()
    {
        _workerRepositoryMock = new Mock<IWorkerRepository>();
        _mapperMock = new Mock<IMapper>();
        _workerService = new WorkerService(_workerRepositoryMock.Object, _mapperMock.Object, null!);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsListOfWorkerDTOs()
    {
        // Arrange
        var workers = new List<Worker> 
        { 
            new Worker() { Name = "1" },
            new Worker() { Name = "2" }
        };
        var workerDtos = new List<GetWorkerDTO>
        { 
            new GetWorkerDTO() { Name = "1" },
            new GetWorkerDTO() { Name = "2" }
        };

        _workerRepositoryMock.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(workers);

        _mapperMock.Setup(mapper => mapper.Map<GetWorkerDTO>(It.IsAny<Worker>()))
            .Returns((Worker worker) => 
                workerDtos.FirstOrDefault(dto => dto.Name == worker.Name)!);

        // Act
        var result = await _workerService.GetAllAsync();

        // Assert
        result.Should().BeEquivalentTo(workerDtos);
    }
}
