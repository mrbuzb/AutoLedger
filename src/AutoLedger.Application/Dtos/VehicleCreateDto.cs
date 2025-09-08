namespace AutoLedger.Application.Dtos;

public class VehicleCreateDto
{
    public string? Make { get; set; }
    public string? Model { get; set; }
    public int? Year { get; set; }
    public string? FuelType { get; set; }
    public string? Plate { get; set; }
}