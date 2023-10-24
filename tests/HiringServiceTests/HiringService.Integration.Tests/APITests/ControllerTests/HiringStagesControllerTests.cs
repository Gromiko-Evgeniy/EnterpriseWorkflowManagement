using FluentAssertions;
using HiringService.Domain.Enumerations;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace HiringService.Integration.Tests.APITests.ControllerTests;
public class HiringStagesControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly JWTGenerator _JWTGenerator;

    public HiringStagesControllerTests()
    {
        var application = new HiringServiceWebApplicationFactory();
        _client = application.CreateClient();

        _JWTGenerator = new JWTGenerator();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsOkResult()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/hiring-stages");
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
        var request = new HttpRequestMessage(HttpMethod.Get, $"/hiring-stages/{id}");
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
        var request = new HttpRequestMessage(HttpMethod.Get, "/hiring-stages/current");
        request.Headers.Add("Authorization",
            "Bearer " + _JWTGenerator.GetToken(
                ApplicationRole.Worker.ToString(),
                "worker1@gmail.com"
            )
        );
        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task MarkAsPassedSuccessfullyAsync_ReturnsNoContent()
    {
        // Arrange
        var id = 1;
        var request = new HttpRequestMessage(HttpMethod.Put, $"/hiring-stages/mark-as-success/{id}");
        request.Headers.Add("Authorization",
            "Bearer " + _JWTGenerator.GetToken(
                ApplicationRole.Worker.ToString(),
                "worker1@gmail.com"
            )
        );

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
