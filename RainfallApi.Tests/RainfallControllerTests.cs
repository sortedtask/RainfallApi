using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using RainfallApi.Controllers;
using RainfallApi.Models;
using System.Net;
using System.Net.Http.Json;

namespace RainfallApi.Tests;

public class RainfallControllerTests
{
    private HttpClient CreateMockHttpClient(HttpResponseMessage responseMessage)
    {
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handlerMock
           .Protected()
           .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.IsAny<HttpRequestMessage>(),
              ItExpr.IsAny<CancellationToken>()
           )
           .ReturnsAsync(responseMessage)
           .Verifiable();

        return new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://test.uk")
        };
    }


    [Fact]
    public async Task GetRainfallReadings_ReturnsOkResult_WithValidParameters()
    {
        // Arrange
        var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(new RainfallReadingResponse
            {
                Readings = new List<RainfallReading>
                    {
                        new RainfallReading
                        {
                            DateMeasured = DateTime.Now,
                            AmountMeasured = 1m
                        }
                    }
            })
        };

        var mockHttpClient = CreateMockHttpClient(mockResponse);
        var controller = new RainfallController(mockHttpClient);

        // Act
        var result = await controller.GetRainfallReadings("559969");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);

        var deserializedResponse = JsonConvert.DeserializeObject<RainfallReadingResponse>(okResult.Value.ToString());

        var response = Assert.IsType<RainfallReadingResponse>(deserializedResponse);
        var readings = Assert.IsAssignableFrom<IEnumerable<RainfallReading>>(response.Readings);
    }

    [Fact]
    public async Task GetRainfallReadings_ReturnsBadRequest_WhenStationIdIsMissing()
    {
        // Arrange
        var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(new RainfallReadingResponse
            {
                Readings = []
            })
        };

        var mockHttpClient = CreateMockHttpClient(mockResponse);
        var controller = new RainfallController(mockHttpClient);

        // Act
        var result = await controller.GetRainfallReadings(null);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        var response = Assert.IsType<ErrorResponse>(badRequestResult.Value);
        Assert.Equal("Invalid request", response.Message);
        Assert.Equal("stationId", response.Detail.First().PropertyName);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    public async Task GetRainfallReadings_ReturnsBadRequest_WhenCountIsOutOfRange(int count)
    {
        // Arrange
        var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(new RainfallReadingResponse
            {
                Readings = []
            })
        };

        var mockHttpClient = CreateMockHttpClient(mockResponse);
        var controller = new RainfallController(mockHttpClient);

        // Act
        var result = await controller.GetRainfallReadings("559969", count);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        var response = Assert.IsType<ErrorResponse>(badRequestResult.Value);
        Assert.Equal("Invalid request", response.Message);
        Assert.Equal("count", response.Detail.First().PropertyName);
    }

    [Fact]
    public async Task GetRainfallReadings_ReturnsNotFound_WhenNoReadingsFound()
    {
        // Arrange
        var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(new RainfallReadingResponse
            {
                Readings = []
            })
        };

        var mockHttpClient = CreateMockHttpClient(mockResponse);
        var controller = new RainfallController(mockHttpClient);

        // Act
        var result = await controller.GetRainfallReadings("559969", 5);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        var response = Assert.IsType<ErrorResponse>(notFoundResult.Value);
        Assert.Equal("No readings found for the specified stationId", response.Message);
    }

    [Fact]
    public async Task GetRainfallReadings_ReturnsServerError_OnHttpRequestException()
    {
        // Arrange
        var mockResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
            Content = JsonContent.Create(new RainfallReadingResponse
            {
                Readings = []
            })
        };

        var mockHttpClient = CreateMockHttpClient(mockResponse);
        var controller = new RainfallController(mockHttpClient);

        // Act
        var result = await controller.GetRainfallReadings("559969", 5);

        // Assert
        var serverErrorResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, serverErrorResult.StatusCode);
        var response = Assert.IsType<ErrorResponse>(serverErrorResult.Value);
        Assert.Equal("Internal server error", response.Message);
    }
}
