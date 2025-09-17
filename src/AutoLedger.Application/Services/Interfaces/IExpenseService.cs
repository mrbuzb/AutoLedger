using AutoLedger.Application.Dtos;
using AutoLedger.Domain.Entities;

namespace AutoLedger.Application.Services.Interfaces;

public interface IExpenseService
{
    Task<long> AddExpenseAsync(ExpenseCreateDto expense,long userId);
    Task DeleteExpenseAsync(long expenseId, long userId);
    Task UpdateExpenseAsync(ExpenseUpdateDto expense,long userId);
    Task<ICollection<ExpenseResponseDto>> GetExpensesByVehicleIdAsync(long vehicleId);
    Task<ExpenseResponseDto> GetExpenseByIdAsync(long expenseId);
    Task<decimal> GetTotalAmountAsync(long vehicleId, DateTime startDate, DateTime endDate);
    Task<ICollection<ExpenseResponseDto>> GetLatestExpensesAsync(long vehicleId, int count = 10);
    Task<ICollection<ExpenseResponseDto>> GetExpensesByCategoryAsync(long vehicleId, long categoryId);
    Task<ICollection<ExpenseResponseDto>> GetExpensesByDateRangeAsync(long vehicleId, DateTime startDate, DateTime endDate);
}
