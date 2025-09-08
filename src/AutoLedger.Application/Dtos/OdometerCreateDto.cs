namespace AutoLedger.Application.Dtos;

public class OdometerCreateDto
{
    public long VehicleId { get; set; }
    public DateTime Date { get; set; }
    public int Value { get; set; }
}