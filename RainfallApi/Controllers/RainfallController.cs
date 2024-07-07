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
            var readings = new List<RainfallReading>();
            return Ok(new RainfallReadingResponse { Readings = readings });
        }
    }
}
