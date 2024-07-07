namespace RainfallApi.Models;

public class ErrorResponse
{
    public required string Message { get; set; }
    public required List<ErrorDetail> Detail { get; set; }
}