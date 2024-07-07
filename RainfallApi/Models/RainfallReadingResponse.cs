using System.Text.Json.Serialization;

namespace RainfallApi.Models;

public class RainfallReadingResponse
{
    [JsonPropertyName("items")]
    public List<RainfallReading> Readings { get; set; }
}