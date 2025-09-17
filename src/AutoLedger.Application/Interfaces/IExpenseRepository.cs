using AutoLedger.Domain.Entities;

namespace AutoLedger.Application.Interfaces;

public interface IExpenseRepository
{
    Task<long> AddExpenseAsync(Expense expense);
    Task DeleteExpenseAsync(long expenseId, long userId);
    Task UpdateExpenseAsync(Expense expense);
    Task<ICollection<Expense>> GetExpensesByVehicleIdAsync(long vehicleId);
    Task<Expense> GetExpenseByIdAsync(long expenseId);
    Task<decimal> GetTotalAmountAsync(long vehicleId, DateTime startDate, DateTime endDate);
    Task<ICollection<Expense>> GetLatestExpensesAsync(long vehicleId, int count = 10);
    Task<ICollection<Expense>> GetExpensesByCategoryAsync(long vehicleId, long categoryId);
    Task<ICollection<Expense>> GetExpensesByDateRangeAsync(long vehicleId, DateTime startDate, DateTime endDate);
}
