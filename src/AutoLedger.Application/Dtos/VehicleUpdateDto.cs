namespace AutoLedger.Application.Dtos;

public class VehicleUpdateDto
{
    public long Id { get; set; }
    public string? Make { get; set; }
    public string? Model { get; set; }
    public int? Year { get; set; }
    public string? FuelType { get; set; }
    public string? Plate { get; set; }
}