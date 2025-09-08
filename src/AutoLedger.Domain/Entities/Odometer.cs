namespace AutoLedger.Domain.Entities;

public class Odometer
{
    public long Id { get; set; }
    public long VehicleId { get; set; }

    public DateTime Date { get; set; }
    public int Value { get; set; }

    public Vehicle Vehicle { get; set; }
}