using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.CQRS.CustomerQueries;
using ProjectManagementService.Application.Exceptions.Customer;
using System.Linq.Expressions;

namespace ProjectManagementService.Unit.Tests.CQRS.QueryTests.CustomerTests;

public class GetCustomerByEmailTests
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly GetCustomerByEmailHandler _handler;

    public GetCustomerByEmailTests()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _handler = new GetCustomerByEmailHandler(_customerRepositoryMock.Object, null!);
    }

    [Fact]
    public async Task Handle_WithExistingCustomerEmail_ReturnsCustomer()
    {
        // Arrange
        var customer = new Customer { Email = "test@example.com" };
        var query = new GetCustomerByEmailQuery(customer.Email);

        _customerRepositoryMock.Setup(r => r.GetFirstAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync(customer);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().Be(customer);
    }

    [Fact]
    public async Task Handle_WithNonExistingCustomerEmail_ThrowsNoCustomerWithSuchEmailException()
    {
        // Arrange
        var query = new GetCustomerByEmailQuery("test@example.com");
        _customerRepositoryMock
            .Setup(r => r.GetFirstAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync((Customer)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NoCustomerWithSuchEmailException>(() => _handler.Handle(query, CancellationToken.None));
    }
}
