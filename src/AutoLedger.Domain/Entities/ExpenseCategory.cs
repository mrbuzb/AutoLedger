namespace AutoLedger.Domain.Entities;

public class ExpenseCategory
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<Expense> Expenses { get; set; }
}