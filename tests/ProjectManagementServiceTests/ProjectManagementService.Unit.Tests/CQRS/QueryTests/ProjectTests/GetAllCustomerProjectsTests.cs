using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.ProjectDTOs;
using ProjectManagementService.Application.Exceptions.Customer;
using ProjectManagementService.Application.CQRS.ProjectQueries;

namespace ProjectManagementService.Unit.Tests.CQRS.QueryTests.ProjectTests;

public class GetAllCustomerProjectsTests
{
    private readonly Mock<IProjectRepository> _projectRepositoryMock;
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllCustomerProjectsHandler _handler;

    public GetAllCustomerProjectsTests()
    {
        _projectRepositoryMock = new Mock<IProjectRepository>();
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetAllCustomerProjectsHandler(
            _projectRepositoryMock.Object,
            _customerRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithExistingCustomerId_ReturnsListOfProjectShortInfoDTO()
    {
        // Arrange
        var customer = new Customer { Id = "1" };
        var query = new GetAllCustomerProjectsQuery(customer.Id);
        var projects = new List<Project>
        { 
            new Project { Id = "1" },
            new Project { Id = "2" } 
        };
        var projectShortInfoDTOs = new List<ProjectShortInfoDTO>
        { 
            new ProjectShortInfoDTO { Id = "1" },
            new ProjectShortInfoDTO { Id = "2" } 
        };

        _customerRepositoryMock
            .Setup(r => r.GetByIdAsync(query.CustomerId))
            .ReturnsAsync(customer);
        _projectRepositoryMock.Setup(r => r.GetAllCustomerProjectsAsync(query.CustomerId))
            .ReturnsAsync(projects);
        _mapperMock.Setup(m => m.Map<ProjectShortInfoDTO>(It.IsAny<Project>()))
            .Returns<Project>(p => new ProjectShortInfoDTO { Id = p.Id });

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(projectShortInfoDTOs);
    }

    [Fact]
    public async Task Handle_WithNonExistingCustomerId_ThrowsNoCustomerWithSuchIdException()
    {
        // Arrange
        var query = new GetAllCustomerProjectsQuery("1");
        _customerRepositoryMock.Setup(r => r.GetByIdAsync(query.CustomerId))
            .ReturnsAsync((Customer)null!);

        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Act & Assert
        await act.Should().ThrowAsync<NoCustomerWithSuchIdException>();
    }
}
