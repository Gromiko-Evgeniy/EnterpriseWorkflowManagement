using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.ProjectDTOs;
using ProjectManagementService.Application.Exceptions.Worker;
using ProjectManagementService.Application.CQRS.ProjectQueries;
using ProjectManagementService.Application.Exceptions.Project;

namespace ProjectManagementService.Unit.Tests.CQRS.QueryTests.ProjectTests;

public class GetProjectByLeaderIdTests
{
    private readonly Mock<IProjectRepository> _projectRepositoryMock;
    private readonly Mock<IWorkerRepository> _workerRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetProjectByLeaderIdHandler _handler;

    public GetProjectByLeaderIdTests()
    {
        _projectRepositoryMock = new Mock<IProjectRepository>();
        _workerRepositoryMock = new Mock<IWorkerRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetProjectByLeaderIdHandler(
            _projectRepositoryMock.Object,
            _workerRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithExistingProjectLeaderId_ReturnsProjectMainInfoDTO()
    {
        // Arrange
        var projectLeader = new Worker { Id = "1" };
        var project = new Project { Id = "1", LeadWorkerId = projectLeader.Id };
        var projectMainInfoDTO = new ProjectMainInfoDTO();
        var query = new GetProjectByLeaderIdQuery(projectLeader.Id);

        _workerRepositoryMock
            .Setup(r => r.GetByIdAsync(projectLeader.Id))
            .ReturnsAsync(projectLeader);

        _projectRepositoryMock
            .Setup(r => r.GetProjectByProjectLeaderId(projectLeader.Id))
            .ReturnsAsync(project);

        _mapperMock
            .Setup(m => m.Map<ProjectMainInfoDTO>(project))
            .Returns(projectMainInfoDTO);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(projectMainInfoDTO);
    }

    [Fact]
    public async Task Handle_WithNonExistingProjectLeaderId_ThrowsNoWorkerWithSuchIdException()
    {
        // Arrange
        var query = new GetProjectByLeaderIdQuery("1");

        _workerRepositoryMock.Setup(r => r.GetByIdAsync(query.ProjectLeaderId))
            .ReturnsAsync((Worker)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NoWorkerWithSuchIdException>(() => _handler.Handle(query, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WithNonExistingProjectForLeader_ThrowsNoProjectWithSuchProjectLeaderException()
    {
        // Arrange
        var projectLeader = new Worker { Id = "1" };
        var query = new GetProjectByLeaderIdQuery(projectLeader.Id);

        _workerRepositoryMock
            .Setup(r => r.GetByIdAsync(projectLeader.Id))
            .ReturnsAsync(projectLeader);

        _projectRepositoryMock
            .Setup(r => r.GetProjectByProjectLeaderId(projectLeader.Id))
            .ReturnsAsync((Project)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NoProjectWithSuchProjectLeaderException>(() => 
            _handler.Handle(query, CancellationToken.None));
    }
}
