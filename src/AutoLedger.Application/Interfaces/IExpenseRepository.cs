using AutoLedger.Domain.Entities;

namespace AutoLedger.Application.Interfaces;

public interface IExpenseRepository
{
    Task<long> AddExpenseAsync(Expense expense);
    Task DeleteExpenseAsync(long expenseId, long userId);
    Task UpdateExpenseAsync(Expense expense);
    Task<ICollection<Expense>> GetExpensesByVehicleIdAsync(long vehicleId);
    Task<Expense> GetExpenseByIdAsync(long expenseId);
}
