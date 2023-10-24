using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.TaskDTOs;
using ProjectManagementService.Application.Exceptions.ProjectTask;
using ProjectManagementService.Application.CQRS.ProjectTaskQueries;

namespace ProjectManagementService.Unit.Tests.CQRS.QueryTests.ProjectTaskTests;

public class GetProjectTaskByIdHandlerTests
{
    private readonly Mock<IProjectTaskRepository> _projectTaskRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetProjectTaskByIdHandler _handler;

    public GetProjectTaskByIdHandlerTests()
    {
        _projectTaskRepositoryMock = new Mock<IProjectTaskRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetProjectTaskByIdHandler(
            _projectTaskRepositoryMock.Object,
            null!,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WithExistingTaskId_ReturnsTaskMainInfoDTO()
    {
        // Arrange
        var task = new ProjectTask { Id = "1" };
        var taskMainInfoDTO = new TaskMainInfoDTO();

        var query = new GetProjectTaskByIdQuery(task.Id);

        _projectTaskRepositoryMock
            .Setup(r => r.GetByIdAsync(query.Id))
            .ReturnsAsync(task);

        _mapperMock
            .Setup(m => m.Map<TaskMainInfoDTO>(task))
            .Returns(taskMainInfoDTO);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(taskMainInfoDTO);
    }

    [Fact]
    public async Task Handle_WithNonExistingTaskId_ThrowsNoProjectTaskWithSuchIdException()
    {
        // Arrange
        var query = new GetProjectTaskByIdQuery("1");

        _projectTaskRepositoryMock.Setup(r => r.GetByIdAsync(query.Id))
            .ReturnsAsync((ProjectTask)null!);

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NoProjectTaskWithSuchIdException>();
    }
}
