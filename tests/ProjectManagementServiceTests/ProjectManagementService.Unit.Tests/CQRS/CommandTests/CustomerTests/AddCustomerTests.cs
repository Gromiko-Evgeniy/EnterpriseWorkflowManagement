using ProjectManagementService.Application.CQRS.CustomerCommands;
using ProjectManagementService.Application.DTOs;

namespace ProjectManagementService.Unit.Tests.CQRS.CommandTests.CustomerTests;

public class AddCustomerTests
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AddCustomerHandler _handler;

    public AddCustomerTests()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new AddCustomerHandler(
            _customerRepositoryMock.Object,
            null!, //cache
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithValidRequest_ReturnsCustomerId()
    {
        // Arrange
        var command = new AddCustomerCommand(new NameEmailDTO());
        var customer = new Customer { Id = "1" };

        _mapperMock.Setup(m => m.Map<Customer>(command.NameEmailDTO))
            .Returns(customer);
        _customerRepositoryMock.Setup(r => r.AddOneAsync(customer))
            .ReturnsAsync(customer.Id);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(customer.Id);
    }
}

