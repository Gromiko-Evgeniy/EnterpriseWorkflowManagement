using HiringService.Domain.Enumerations;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using FluentAssertions;
using HiringService.Application.DTOs.CandidateDTOs;

namespace HiringService.Integration.Tests.APITests.ControllerTests;

public class CandidatesControllerTests
{
    private readonly HttpClient _client;
    private readonly JWTGenerator _JWTGenerator;

    public CandidatesControllerTests()
    {
        var application = new HiringServiceWebApplicationFactory();
        _client = application.CreateClient();

        _JWTGenerator = new JWTGenerator();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsOkResult()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/candidates");
        request.Headers.Add("Authorization",
            "Bearer " + _JWTGenerator.GetToken(
                ApplicationRole.DepartmentHead.ToString(),
                ""
            )
        );

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsOkResult()
    {
        // Arrange
        var id = 1;
        var request = new HttpRequestMessage(HttpMethod.Get, $"/candidates/{id}");
        request.Headers.Add("Authorization",
            "Bearer " + _JWTGenerator.GetToken(
                ApplicationRole.DepartmentHead.ToString(),
                ""
            )
        );

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetByEmailAsync_ReturnsOkResult()
    {
        // Arrange
        var email = "candidate1@gmail.com";
        var request = new HttpRequestMessage(HttpMethod.Get, $"/candidates/{email}");
        request.Headers.Add("Authorization",
            "Bearer " + _JWTGenerator.GetToken(
                ApplicationRole.DepartmentHead.ToString(),
                ""
            )
        );
        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetCurrentAsync_ReturnsOkResult()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/candidates/current");
        request.Headers.Add("Authorization",
            "Bearer " + _JWTGenerator.GetToken(
                ApplicationRole.Candidate.ToString(),
                "candidate1@gmail.com"
            )
        );
        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task AddAsync_ReturnsOkResult()
    {
        // Arrange
        var candidate = new AddCandidateDTO 
        { 
            Name = "candidate100",
            Email = "candidate100@gmail.com",
            CV = "i am candidate100",
        };

        var content = new StringContent(JsonConvert.SerializeObject(candidate), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/candidates", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateNameAsync_ReturnsNoContentResult()
    {
        // Arrange
        var name = "New Name";
        var request = new HttpRequestMessage(HttpMethod.Put, "/candidates/new-name");
        request.Headers.Add("Authorization",
            "Bearer " + _JWTGenerator.GetToken(
                ApplicationRole.Candidate.ToString(),
                "candidate1@gmail.com"
            )
        );
        request.Content = new StringContent(JsonConvert.SerializeObject(name), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task UpdateCVAsync_ReturnsNoContentResult()
    {
        // Arrange
        var cv = "New CV";
        var request = new HttpRequestMessage(HttpMethod.Put, "/candidates/new-cv");
        request.Headers.Add("Authorization",
            "Bearer " + _JWTGenerator.GetToken(
                ApplicationRole.Candidate.ToString(),
                "candidate1@gmail.com"
            )
        );
        request.Content = new StringContent(JsonConvert.SerializeObject(cv), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
