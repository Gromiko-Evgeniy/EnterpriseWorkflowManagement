using IdentityService.Application.DTOs;
using IdentityService.Application.DTOs.CandidateDTO;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace IdentityService.Integration.Tests.APITests.ControllerTests;

public class CandidatesControllerTests
{
    private readonly HttpClient _client;

    public CandidatesControllerTests()
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
            Email = "candidater1@gmail.com",
            Password = "1234",
        };
        var content = new StringContent(JsonConvert.SerializeObject(logInData), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/candidates/log-in", content);
        var token = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Registration_ReturnsOkResultWithToken()
    {
        // Arrange
        var candidateDTO = new AddCandidateDTO 
        { 
            CV = "i am new",
            Name = "New",
            Email = "newemail@gmial.com",
            Password = "1234"

        };
        var content = new StringContent(JsonConvert.SerializeObject(candidateDTO), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/candidates/sign-in", content);
        var token = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        token.Should().NotBeNullOrEmpty();
    }
}
