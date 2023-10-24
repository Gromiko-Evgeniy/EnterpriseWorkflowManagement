using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.CQRS.ProjectTaskQueries;
using ProjectManagementService.Application.TaskDTOs;

namespace ProjectManagementService.Unit.Tests.CQRS.QueryTests.ProjectTaskTests;

public class GetAllProjectTasksHandlerTests
{
    private readonly Mock<IProjectTaskRepository> _projectTaskRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllProjectTasksHandler _handler;

    public GetAllProjectTasksHandlerTests()
    {
        _projectTaskRepositoryMock = new Mock<IProjectTaskRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetAllProjectTasksHandler(_projectTaskRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsListOfTaskShortInfoDTO()
    {
        // Arrange
        var query = new GetAllProjectTasksQuery();
        var tasks = new List<ProjectTask>
        {
            new ProjectTask { Id = "1" },
            new ProjectTask { Id = "2" }
        };
        var taskShortInfoDTOs = new List<TaskShortInfoDTO> 
        { 
            new TaskShortInfoDTO { Id = "1" },
            new TaskShortInfoDTO { Id = "2" }
        };

        _projectTaskRepositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(tasks);
        _mapperMock
            .Setup(m => m.Map<TaskShortInfoDTO>(It.IsAny<ProjectTask>()))
            .Returns<ProjectTask>(t => new TaskShortInfoDTO { Id = t.Id });

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(taskShortInfoDTOs);
    }
}
