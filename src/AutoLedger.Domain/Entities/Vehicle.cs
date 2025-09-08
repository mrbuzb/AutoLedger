namespace AutoLedger.Domain.Entities;

public class Vehicle
{
    public long Id { get; set; }
    public long UserId { get; set; }

    public string? Make { get; set; }  
    public string? Model { get; set; } 
    public int? Year { get; set; }
    public string? FuelType { get; set; }
    public string? Plate { get; set; } 

    public DateTime CreatedAt { get; set; }

    public User User { get; set; }
    public ICollection<Expense> Expenses { get; set; }
    public ICollection<Odometer> Odometers { get; set; }
}
