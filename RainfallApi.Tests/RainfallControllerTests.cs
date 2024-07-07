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
}
