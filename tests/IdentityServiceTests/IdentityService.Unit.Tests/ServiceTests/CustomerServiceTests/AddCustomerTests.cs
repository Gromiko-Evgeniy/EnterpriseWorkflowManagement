using IdentityService.Application.DTOs;
using IdentityService.Application.DTOs.CustomerDTOs;
using IdentityService.Application.Exceptions.Customer;
using IdentityService.Application.KafkaAbstractions;

namespace IdentityService.Unit.Tests.ServiceTests.CustomerServiceTests;

public class AddCustomerTests
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly Mock<IKafkaProducer> _kafkaProducerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CustomerService _customerService;

    public AddCustomerTests()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _kafkaProducerMock = new Mock<IKafkaProducer>();
        _mapperMock = new Mock<IMapper>();
        _customerService = new CustomerService(_customerRepositoryMock.Object, _mapperMock.Object, _kafkaProducerMock.Object);
    }

    [Fact]
    public async Task AddAsync_WhenCustomerDoesNotExist_AddsCustomerAndReturnsLogInData()
    {
        // Arrange
        var customerDTO = new AddCustomerDTO { Email = "test@example.com", Password = "password123" };
        var newCustomer = new Customer { Email = customerDTO.Email, Password = customerDTO.Password };
        var logInData = new LogInData { Email = customerDTO.Email, Password = customerDTO.Password };

        _customerRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync((Customer)null);
        _mapperMock.Setup(mapper => mapper.Map<Customer>(customerDTO))
            .Returns(newCustomer);
        _mapperMock.Setup(mapper => mapper.Map<LogInData>(newCustomer))
            .Returns(logInData);

        // Act
        var result = await _customerService.AddAsync(customerDTO);

        // Assert
        _customerRepositoryMock.Verify(repo => repo.Add(newCustomer), Times.Once);
        _customerRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        _kafkaProducerMock.Verify(producer => producer.SendAddCustomerMessage(It.IsAny<NameEmailDTO>()), Times.Once);
        result.Should().BeEquivalentTo(logInData);
    }

    [Fact]
    public async Task AddAsync_WhenCustomerExists_ThrowsCustomerAlreadyExistsException()
    {
        // Arrange
        var customerDTO = new AddCustomerDTO { Email = "test@example.com", Password = "password123" };
        var existingCustomer = new Customer { Email = customerDTO.Email, Password = customerDTO.Password };

        _customerRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync(existingCustomer);

        // Act
        Func<Task> act = async () => await _customerService.AddAsync(customerDTO);

        // Assert
        await act.Should().ThrowAsync<CustomerAlreadyExistsException>();
        _customerRepositoryMock.Verify(repo => repo.Add(It.IsAny<Customer>()), Times.Never);
        _customerRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        _kafkaProducerMock.Verify(producer => producer.SendAddCustomerMessage(It.IsAny<NameEmailDTO>()), Times.Never);
    }
}
