using AutoLedger.Application.Dtos;
using AutoLedger.Application.Interfaces;
using AutoLedger.Application.Services.Interfaces;
using AutoLedger.Core.Errors;
using AutoLedger.Domain.Entities;

namespace AutoLedger.Application.Services.Implementations;

public class ExpenseService(IExpenseRepository _repo) : IExpenseService
{
    public async Task<long> AddExpenseAsync(ExpenseCreateDto expense, long userId)
    {
        return await _repo.AddExpenseAsync(new Expense
        {
            Amount = expense.Amount,
            CategoryId = expense.CategoryId,
            CreatedAt = DateTime.Now,
            Date = expense.Date,
            Note = expense.Note,
            Odometer = expense.Odometer,
            Quantity = expense.Quantity,
            VehicleId = expense.VehicleId,
        });
    }

    public async Task DeleteExpenseAsync(long expenseId, long userId)
    {
        await _repo.DeleteExpenseAsync(expenseId, userId);
    }

    public async Task<ExpenseResponseDto> GetExpenseByIdAsync(long expenseId)
    {
        return Converter(await _repo.GetExpenseByIdAsync(expenseId));
    }

    public async Task<ICollection<ExpenseResponseDto>> GetExpensesByCategoryAsync(long vehicleId, long categoryId)
    {
        var expense = await _repo.GetExpensesByCategoryAsync(vehicleId, categoryId);
        return expense.Select(Converter).ToList();
    }

    public async Task<ICollection<ExpenseResponseDto>> GetExpensesByDateRangeAsync(long vehicleId, DateTime startDate, DateTime endDate)
    {
        var expense = await _repo.GetExpensesByDateRangeAsync(vehicleId, startDate, endDate);
        return expense.Select(Converter).ToList();
    }

    public async Task<ICollection<ExpenseResponseDto>> GetExpensesByVehicleIdAsync(long vehicleId)
    {
        var expenses = await _repo.GetExpensesByVehicleIdAsync(vehicleId);
        return expenses.Select(Converter).ToList();
    }

    public async Task<ICollection<ExpenseResponseDto>> GetLatestExpensesAsync(long vehicleId, int count = 10)
    {
        var expenses = await _repo.GetLatestExpensesAsync(vehicleId, count);
        if (count > 30) count = 10;
        return expenses.Select(Converter).ToList();
    }

    public async Task<decimal> GetTotalAmountAsync(long vehicleId, DateTime startDate, DateTime endDate)
    {
        return await _repo.GetTotalAmountAsync(vehicleId, startDate, endDate);
    }

    public async Task UpdateExpenseAsync(ExpenseUpdateDto expense, long userId)
    {
        var entity = await _repo.GetExpenseByIdAsync(userId);
        if (entity.Vehicle.UserId != userId)
        {
            throw new NotAllowedException();
        }
        entity.Note = expense.Note;
        entity.Odometer = expense.Odometer;
        entity.Quantity = expense.Quantity;
        entity.Amount = expense.Amount;
        entity.CategoryId = expense.CategoryId;
        entity.Date = expense.Date;
        entity.CategoryId = expense.CategoryId;
        await _repo.UpdateExpenseAsync(entity);
    }

    private ExpenseResponseDto Converter(Expense exp)
    {
        return new ExpenseResponseDto()
        {
            Amount = exp.Amount,
            CategoryId = exp.CategoryId,
            CreatedAt = exp.CreatedAt,
            Id = exp.Id,
            Date = exp.Date,
            Note = exp.Note,
            Odometer = exp.Odometer,
            Quantity = exp.Quantity,
            VehicleId = exp.VehicleId,
            CategoryName = exp.Category.Name,
        };
    }
}
