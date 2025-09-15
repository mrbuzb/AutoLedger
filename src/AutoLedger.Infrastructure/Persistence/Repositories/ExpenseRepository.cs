using AutoLedger.Application.Interfaces;
using AutoLedger.Core.Errors;
using AutoLedger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoLedger.Infrastructure.Persistence.Repositories;

public class ExpenseRepository(AppDbContext _context) : IExpenseRepository
{
    public async Task<long> AddExpenseAsync(Expense expense)
    {
        await _context.Expenses.AddAsync(expense);
        await _context.SaveChangesAsync();
        return expense.Id;
    }

    public async Task DeleteExpenseAsync(long expenseId, long userId)
    {
        var expense = await _context.Expenses.Include(x=>x.Vehicle).FirstOrDefaultAsync(x=>x.Id == expenseId);
        if(expense == null || expense.Vehicle.UserId != userId)
        {
            throw new NotAllowedException();
        }
        _context.Expenses.Remove(expense);
        await _context.SaveChangesAsync();
    }

    public async Task<Expense> GetExpenseByIdAsync(long expenseId)
    {
        var expense = await _context.Expenses.FindAsync(expenseId);
        if (expense == null)
        {
            throw new EntityNotFoundException($"Expense not found with id {expenseId}");
        }
        return expense;
    }

    public async Task<ICollection<Expense>> GetExpensesByVehicleIdAsync(long vehicleId)
    {
        return await _context.Expenses.Where(x=>x.VehicleId == vehicleId).ToListAsync();
    }

    public async Task UpdateExpenseAsync(Expense expense)
    {
        _context.Expenses.Update(expense);
        await _context.SaveChangesAsync();
    }
}
