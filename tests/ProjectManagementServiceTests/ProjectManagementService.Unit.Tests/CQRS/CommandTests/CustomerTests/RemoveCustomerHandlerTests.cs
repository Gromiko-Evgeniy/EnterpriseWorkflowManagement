using Moq;
using ProjectManagementService.Application.CQRS.CustomerCommands;
using ProjectManagementService.Application.Exceptions.Customer;
using ProjectManagementService.Domain.Entities;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ProjectManagementService.Unit.Tests.CQRS.CommandTests.CustomerTests;

public class RemoveCustomerHandlerTests
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly Mock<IProjectRepository> _projectRepositoryMock;
    private readonly Mock<IProjectTaskRepository> _projectTaskRepositoryMock;
    private readonly RemoveCustomerHandler _handler;

    public RemoveCustomerHandlerTests()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _projectRepositoryMock = new Mock<IProjectRepository>();
        _projectTaskRepositoryMock = new Mock<IProjectTaskRepository>();
        _handler = new RemoveCustomerHandler(
            _customerRepositoryMock.Object,
            _projectRepositoryMock.Object,
            _projectTaskRepositoryMock.Object,
            null! // cache
        );
    }

    [Fact]
    public async Task Handle_WithExistingCustomerEmail_RemovesCustomerAndRelatedProjectsAndTasks()
    {
        // Arrange
        var customer = new Customer { Id = "1", Email = "vasya@example.com" };

        var command = new RemoveCustomerCommand(customer.Email);
        var projects = new List<Project>
        {
            new Project { Id = "1", CustomerId = customer.Id },
            new Project { Id = "2", CustomerId = customer.Id }
        };
        var tasks = new List<ProjectTask>
        {
            new ProjectTask { Id = "1", ProjectId = "1" },
            new ProjectTask { Id = "2", ProjectId = "1" },
            new ProjectTask { Id = "3", ProjectId = "2" }
        };

        _customerRepositoryMock
            .Setup(r => r.GetFirstAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync(customer);

        _projectRepositoryMock
            .Setup(r => r.GetFilteredAsync(It.IsAny<Expression<Func<Project, bool>>>()))
            .ReturnsAsync(projects);

        _projectTaskRepositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(tasks);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _customerRepositoryMock.Verify(r => r.RemoveAsync(customer.Id), Times.Once);
        _projectRepositoryMock.Verify(r => r.CancelAsync(projects[0].Id), Times.Once);
        _projectRepositoryMock.Verify(r => r.CancelAsync(projects[1].Id), Times.Once);
        _projectTaskRepositoryMock.Verify(r => r.CancelAsync(tasks[0].Id), Times.Once);
        _projectTaskRepositoryMock.Verify(r => r.CancelAsync(tasks[1].Id), Times.Once);
        _projectTaskRepositoryMock.Verify(r => r.CancelAsync(tasks[2].Id), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistingCustomerEmail_ThrowsNoCustomerWithSuchIdException()
    {
        // Arrange
        var command = new RemoveCustomerCommand("vasya@example.com");

        _customerRepositoryMock
            .Setup(r => r.GetFirstAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync((Customer)null!);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NoCustomerWithSuchIdException>();

        _customerRepositoryMock.Verify(r => r.RemoveAsync(It.IsAny<string>()), Times.Never);
        _projectRepositoryMock.Verify(r => r.CancelAsync(It.IsAny<string>()), Times.Never);
        _projectTaskRepositoryMock.Verify(r => r.CancelAsync(It.IsAny<string>()), Times.Never);
    }
}

