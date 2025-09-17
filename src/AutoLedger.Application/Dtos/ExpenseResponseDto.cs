namespace AutoLedger.Application.Dtos;

public class ExpenseResponseDto
{
    public long Id { get; set; }
    public long VehicleId { get; set; }
    public long CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;

    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public int? Odometer { get; set; }
    public decimal? Quantity { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; }
}