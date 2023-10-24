using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using HiringService.Application.DTOs.StageNameDTOs;
using HiringService.Domain.Enumerations;
using Newtonsoft.Json;
using System.Text;
using FluentAssertions;

namespace HiringService.Integration.Tests.APITests.ControllerTests;

public class HiringStageNamesControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly JWTGenerator _JWTGenerator;

    public HiringStageNamesControllerTests()
    {
        var application = new HiringServiceWebApplicationFactory();
        _client = application.CreateClient();

        _JWTGenerator = new JWTGenerator();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsOkResult()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/hiring-stage-names");
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
        var request = new HttpRequestMessage(HttpMethod.Get, $"/hiring-stage-names/{id}");
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
    public async Task GetByNameAsync_ReturnsOkResult()
    {
        // Arrange
        var name = "English";
        var request = new HttpRequestMessage(HttpMethod.Get, $"/hiring-stage-names/{name}");
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
    public async Task AddAsync_ReturnsOkResult()
    {
        // Arrange
        var hiringStageDTO = new AddStageNameDTO 
        { 
            Name = "NewStage",
            Index = 1
        };

        var request = new HttpRequestMessage(HttpMethod.Post, "/hiring-stage-names");
        request.Content = new StringContent(JsonConvert.SerializeObject(hiringStageDTO), Encoding.UTF8, "application/json");
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
    public async Task RemoveAsync_ReturnsNoContentResult()
    {
        // Arrange
        var id = 1;
        var request = new HttpRequestMessage(HttpMethod.Delete, $"/hiring-stage-names/{id}");
        request.Headers.Add("Authorization",
            "Bearer " + _JWTGenerator.GetToken(
                ApplicationRole.DepartmentHead.ToString(),
                ""
            )
        );
        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}

