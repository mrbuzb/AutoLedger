using AutoLedger.Domain.Entities;

namespace AutoLedger.Application.Interfaces;

public interface IExspenseCategory
{
    Task<ExpenseCategory?> SelectExpenseCategoryByIdAsync(long id);
    Task<ExpenseCategory?> SelectExpenseCategoryByNameAsync(string name);
    Task<ICollection<ExpenseCategory>> SelectAllExpenseCategoriesAsync();
    Task<long> AddExpenseCategoryAsync(ExpenseCategory expenseCategory);
    Task UpdateExpenseCategoryAsync(ExpenseCategory expenseCategory);
    Task DeleteExpenseCategory(long id);
}
