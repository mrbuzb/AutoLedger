namespace AutoLedger.Domain.Entities;

public class Expense
{
    public long Id { get; set; }
    public long VehicleId { get; set; }
    public long CategoryId { get; set; }

    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public int? Odometer { get; set; }
    public decimal? Quantity { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; }
    public Vehicle Vehicle { get; set; }

    public ExpenseCategory Category { get; set; }
}
