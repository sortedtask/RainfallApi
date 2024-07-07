using Microsoft.AspNetCore.Mvc;
using RainfallApi.Models;

namespace RainfallApi.Controllers
{
    [ApiController]
    [Route("rainfall/id/{stationId}/readings")]
    public class RainfallController : ControllerBase
    {
        [HttpGet]
        public ActionResult<RainfallReadingResponse> GetRainfallReadings(string stationId, [FromQuery] int count = 10)
        {
            var stationIdIsMissing = string.IsNullOrEmpty(stationId);
            if (stationIdIsMissing)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Invalid request",
                    Detail =
                    [
                        new ErrorDetail { PropertyName = "stationId", Message = "Station ID is required" }
                    ]
                });
            }

            var readings = new List<RainfallReading>();
            return Ok(new RainfallReadingResponse { Readings = readings });
        }
    }
}
