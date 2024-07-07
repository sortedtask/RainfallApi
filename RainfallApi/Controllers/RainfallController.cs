using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RainfallApi.Models;

namespace RainfallApi.Controllers
{
    [ApiController]
    [Route("rainfall/id/{stationId}/readings")]
    public class RainfallController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public RainfallController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<ActionResult<RainfallReadingResponse>> GetRainfallReadings(string stationId, [FromQuery] int count = 10)
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

            var minimumCount = 1;
            var maximumCount = 100;

            var requestedCountIsOutsideValidRange = count < minimumCount || count > maximumCount;
            if (requestedCountIsOutsideValidRange)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Invalid request",
                    Detail =
                    [
                        new ErrorDetail { PropertyName = "count", Message = $"Count must be between {minimumCount} and {maximumCount}" }
                    ]
                });
            }

            var url = $"https://environment.data.gov.uk/flood-monitoring/id/stations/{stationId}/readings?_sorted&_limit={count}";

            try
            {
                var response = await _httpClient.GetFromJsonAsync<RainfallReadingResponse>(url);

                if (response == null || response.Readings == null || response.Readings.Count == 0)
                {
                    return NotFound(new ErrorResponse
                    {
                        Message = "No readings found for the specified stationId",
                        Detail = new List<ErrorDetail>()
                    });
                }

                var serializeSettings = new JsonSerializerSettings();
                serializeSettings.Formatting = Formatting.Indented;
                serializeSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                var responseString = JsonConvert.SerializeObject(response, serializeSettings);

                return Ok(responseString);
            }
            catch (HttpRequestException)
            {
                return StatusCode(500, new ErrorResponse
                {
                    Message = "Internal server error",
                    Detail = new List<ErrorDetail>()
                });
            }
        }
    }
}
