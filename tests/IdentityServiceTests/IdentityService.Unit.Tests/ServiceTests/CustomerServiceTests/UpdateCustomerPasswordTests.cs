using IdentityService.Application.Exceptions;
using IdentityService.Application.Exceptions.Customer;

namespace IdentityService.Unit.Tests.ServiceTests.CustomerServiceTests;

public class UpdateCustomerPasswordTests
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly CustomerService _customerService;

    public UpdateCustomerPasswordTests()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _customerService = new CustomerService(_customerRepositoryMock.Object, null!, null!);
    }

    [Fact]
    public async Task UpdatePasswordAsync_WhenCustomerExistsAndPasswordsMatch_UpdatesPassword()
    {
        // Arrange
        var email = "test@example.com";
        var prevPassword = "password123";
        var newPassword = "newpassword456";
        var customer = new Customer { Email = email, Password = prevPassword };

        _customerRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync(customer);

        // Act
        await _customerService.UpdatePasswordAsync(email, prevPassword, newPassword);

        // Assert
        customer.Password.Should().Be(newPassword);
        _customerRepositoryMock.Verify(repo => repo.Update(customer), Times.Once);
        _customerRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdatePasswordAsync_WhenCustomerDoesNotExist_ThrowsNoCustomerWithSuchEmailException()
    {
        // Arrange
        var email = "test@example.com";
        var prevPassword = "password123";
        var newPassword = "newpassword456";

        _customerRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync((Customer)null);

        // Act
        Func<Task> act = async () => await _customerService.UpdatePasswordAsync(email, prevPassword, newPassword);

        // Assert
        await act.Should().ThrowAsync<NoCustomerWithSuchEmailException>();
        _customerRepositoryMock.Verify(repo => repo.Update(It.IsAny<Customer>()), Times.Never);
        _customerRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task UpdatePasswordAsync_WhenPasswordsDoNotMatch_ThrowsIncorrectPasswordException()
    {
        // Arrange
        var email = "test@example.com";
        var prevPassword = "password123";
        var newPassword = "newpassword456";
        var customer = new Customer { Email = email, Password = "wrongpassword" };

        _customerRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync(customer);

        // Act
        Func<Task> act = async () => await _customerService.UpdatePasswordAsync(email, prevPassword, newPassword);

        // Assert
        await act.Should().ThrowAsync<IncorrectPasswordException>();
        _customerRepositoryMock.Verify(repo => repo.Update(It.IsAny<Customer>()), Times.Never);
        _customerRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
    }
}
