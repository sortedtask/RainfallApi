namespace RainfallApi.Models;

public class ErrorDetail
{
    public required string PropertyName { get; set; }
    public required string Message { get; set; }
}