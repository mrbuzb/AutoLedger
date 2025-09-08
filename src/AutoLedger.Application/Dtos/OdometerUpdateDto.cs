namespace AutoLedger.Application.Dtos;

public class OdometerUpdateDto
{
    public long OdometerId { get; set; }
    public DateTime Date { get; set; }
    public int Value { get; set; }
}