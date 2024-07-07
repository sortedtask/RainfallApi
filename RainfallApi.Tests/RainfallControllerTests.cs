using Microsoft.AspNetCore.Mvc;
using RainfallApi.Controllers;
using RainfallApi.Models;

namespace RainfallApi.Tests;

public class RainfallControllerTests
{
    [Fact]
    public void GetRainfallReadings_ReturnsOkResult_WithValidParameters()
    {
        // Arrange
        var controller = new RainfallController();

        // Act
        var result = controller.GetRainfallReadings("559969");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<RainfallReadingResponse>(okResult.Value);
        var readings = Assert.IsAssignableFrom<IEnumerable<RainfallReading>>(response.Readings);
    }

    [Fact]
    public void GetRainfallReadings_ReturnsBadRequest_WhenStationIdIsMissing()
    {
        // Arrange
        var controller = new RainfallController();

        // Act
        var result = controller.GetRainfallReadings(null);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        var response = Assert.IsType<ErrorResponse>(badRequestResult.Value);
        Assert.Equal("Invalid request", response.Message);
        Assert.Equal("stationId", response.Detail.First().PropertyName);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    public void GetRainfallReadings_ReturnsBadRequest_WhenCountIsOutOfRange(int count)
    {
        // Arrange
        var controller = new RainfallController();

        // Act
        var result = controller.GetRainfallReadings("station123", count);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        var response = Assert.IsType<ErrorResponse>(badRequestResult.Value);
        Assert.Equal("Invalid request", response.Message);
        Assert.Equal("count", response.Detail.First().PropertyName);
    }
}
