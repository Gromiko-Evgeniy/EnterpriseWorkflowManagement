using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.TaskDTOs;
using ProjectManagementService.Application.Exceptions.ProjectTask;
using ProjectManagementService.Application.CQRS.ProjectTaskQueries;

namespace ProjectManagementService.Unit.Tests.CQRS.QueryTests.ProjectTaskTests;

public class GetProjectTaskByWorkerIdHandlerTests
{
    private readonly Mock<IProjectTaskRepository> _projectTaskRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetProjectTaskByWorkerIdHandler _handler;

    public GetProjectTaskByWorkerIdHandlerTests()
    {
        _projectTaskRepositoryMock = new Mock<IProjectTaskRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetProjectTaskByWorkerIdHandler(
            _projectTaskRepositoryMock.Object,
            null!,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WithExistingWorkerId_ReturnsTaskMainInfoDTO()
    {
        // Arrange
        var workerTask = new ProjectTask { Id = "1", WorkerId = "1" };
        var taskMainInfoDTO = new TaskMainInfoDTO();
        var query = new GetProjectTaskByWorkerIdQuery(workerTask.WorkerId);

        _projectTaskRepositoryMock.Setup(r => r.GetByWorkerIdAsync(query.WorkerId))
            .ReturnsAsync(workerTask);
        _mapperMock.Setup(m => m.Map<TaskMainInfoDTO>(workerTask))
            .Returns(taskMainInfoDTO);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(taskMainInfoDTO);
    }

    [Fact]
    public async Task Handle_WithNonExistingWorkerId_ThrowsNoTaskWithSuchWorkerIdException()
    {
        // Arrange
        var query = new GetProjectTaskByWorkerIdQuery("1");

        _projectTaskRepositoryMock.Setup(r => r.GetByWorkerIdAsync(query.WorkerId))
            .ReturnsAsync((ProjectTask)null!);

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NoTaskWithSuchWorkerIdException>();
    }
}

