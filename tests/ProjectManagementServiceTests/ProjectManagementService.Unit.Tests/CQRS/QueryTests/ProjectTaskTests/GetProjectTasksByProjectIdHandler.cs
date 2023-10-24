using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.TaskDTOs;
using ProjectManagementService.Application.Exceptions.Project;
using ProjectManagementService.Application.CQRS.ProjectTaskQueries;

namespace ProjectManagementService.Unit.Tests.CQRS.QueryTests.ProjectTaskTests;

public class GetProjectTasksByProjectIdHandlerTests
{
    private readonly Mock<IProjectTaskRepository> _projectTaskRepositoryMock;
    private readonly Mock<IProjectRepository> _projectRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetProjectTasksByProjectIdHandler _handler;

    public GetProjectTasksByProjectIdHandlerTests()
    {
        _projectTaskRepositoryMock = new Mock<IProjectTaskRepository>();
        _projectRepositoryMock = new Mock<IProjectRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetProjectTasksByProjectIdHandler(
            _projectTaskRepositoryMock.Object,
            _projectRepositoryMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WithExistingProjectId_ReturnsListOfTaskShortInfoDTO()
    {
        // Arrange
        var project = new Project { Id = "1" };
        var query = new GetProjectTasksByProjectIdQuery(project.Id);

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

        _projectRepositoryMock.Setup(r => r.GetByIdAsync(query.ProjectId))
            .ReturnsAsync(project);
        _projectTaskRepositoryMock.Setup(r => r.GetByProjectIdAsync(query.ProjectId))
            .ReturnsAsync(tasks);
        _mapperMock.Setup(m => m.Map<TaskShortInfoDTO>(It.IsAny<ProjectTask>()))
            .Returns<ProjectTask>(t => taskShortInfoDTOs.FirstOrDefault(dto => dto.Id == t.Id)!);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(taskShortInfoDTOs);
    }

    [Fact]
    public async Task Handle_WithNonExistingProjectId_ThrowsNoProjectWithSuchIdException()
    {
        // Arrange
        var query = new GetProjectTasksByProjectIdQuery("1");

        _projectRepositoryMock.Setup(r => r.GetByIdAsync(query.ProjectId))
            .ReturnsAsync((Project)null!);

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NoProjectWithSuchIdException>();
    }
}

