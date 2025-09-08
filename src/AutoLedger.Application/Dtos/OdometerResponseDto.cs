namespace AutoLedger.Application.Dtos;

public class OdometerResponseDto
{
    public long Id { get; set; }
    public long VehicleId { get; set; }
    public DateTime Date { get; set; }
    public int Value { get; set; }
}