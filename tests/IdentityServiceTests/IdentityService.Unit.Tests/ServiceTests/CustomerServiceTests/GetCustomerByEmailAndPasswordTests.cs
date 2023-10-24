using IdentityService.Application.DTOs;
using IdentityService.Application.Exceptions;
using IdentityService.Application.Exceptions.Customer;

namespace IdentityService.Unit.Tests.ServiceTests.CustomerServiceTests;

public class GetCustomerByEmailAndPasswordTests
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly CustomerService _customerService;

    public GetCustomerByEmailAndPasswordTests()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _customerService = new CustomerService(_customerRepositoryMock.Object, null!, null!);
    }

    [Fact]
    public async Task GetByEmailAndPasswordAsync_WhenCustomerExistsAndPasswordsMatch_ReturnsCustomer()
    {
        // Arrange
        var email = "test@example.com";
        var password = "password123";
        var customer = new Customer { Email = email, Password = password };
        var logInData = new LogInData { Email = email, Password = password };

        _customerRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync(customer);

        // Act
        var result = await _customerService.GetByEmailAndPasswordAsync(logInData);

        // Assert
        result.Should().BeEquivalentTo(customer);
    }

    [Fact]
    public async Task GetByEmailAndPasswordAsync_WhenCustomerDoesNotExist_ThrowsNoCustomerWithSuchEmailException()
    {
        // Arrange
        var email = "test@example.com";
        var password = "password123";
        var logInData = new LogInData { Email = email, Password = password };

        _customerRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync((Customer)null);

        // Act
        Func<Task> act = async () => await _customerService.GetByEmailAndPasswordAsync(logInData);

        // Assert
        await act.Should().ThrowAsync<NoCustomerWithSuchEmailException>();
    }

    [Fact]
    public async Task GetByEmailAndPasswordAsync_WhenPasswordsDoNotMatch_ThrowsIncorrectPasswordException()
    {
        // Arrange
        var email = "test@example.com";
        var password = "password123";
        var customer = new Customer { Email = email, Password = "wrongpassword" };
        var logInData = new LogInData { Email = email, Password = password };

        _customerRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync(customer);

        // Act
        Func<Task> act = async () => await _customerService.GetByEmailAndPasswordAsync(logInData);

        // Assert
        await act.Should().ThrowAsync<IncorrectPasswordException>();
    }
}
