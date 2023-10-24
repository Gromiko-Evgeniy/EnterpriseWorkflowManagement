using IdentityService.Application.DTOs;
using IdentityService.Application.DTOs.CustomerDTOs;
using System.Net.Http.Json;
using System.Net;

namespace IdentityService.Integration.Tests.APITests.ControllerTests;

public class CustomersControllerTests
{
    private readonly HttpClient _client;

    public CustomersControllerTests()
    {
        var application = new IdentityServiceWebApplicationFactory();
        _client = application.CreateClient();
    }


    [Fact]
    public async Task LogIn_ReturnsOkResultWithToken()
    {
        // Arrange
        var logInData = new LogInData 
        {
            Email = "customer1@gmail.com",
            Password = "1234",
        };

        // Act
        var response = await _client.PostAsJsonAsync("/customers/log-in", logInData);
        var token = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Registration_ReturnsOkResultWithToken()
    {
        // Arrange
        var customerDTO = new AddCustomerDTO 
        {
            Name = "New",
            Email = "newemail@gmial.com",
            Password = "1234"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/customers/sign-in", customerDTO);
        var token = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        token.Should().NotBeNullOrEmpty();
    }
}
