using System.Text.Json.Serialization;

namespace RainfallApi.Models;

public class RainfallReading
{
    [JsonPropertyName("dateTime")]
    public DateTime DateMeasured { get; set; }

    [JsonPropertyName("value")]
    public decimal AmountMeasured { get; set; }
}