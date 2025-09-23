namespace AutoLedger.Application.Dtos;

public class ExpenseUpdateDto
{
    public long ExpenseId { get; set; }
    public long CategoryId { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public int? Odometer { get; set; }
    public decimal? Quantity { get; set; }
    public string? Note { get; set; }
}
