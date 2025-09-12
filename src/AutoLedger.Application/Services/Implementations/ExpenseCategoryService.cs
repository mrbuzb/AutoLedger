using AutoLedger.Application.Dtos;
using AutoLedger.Application.Interfaces;
using AutoLedger.Application.Services.Interfaces;
using AutoLedger.Domain.Entities;

namespace AutoLedger.Application.Services.Implementations;

public class ExpenseCategoryService(IExpenseCategoryRepository expenseCategoryRepository) : IExpenseCategoryService
{
    public async Task<long> AddExpenseCategoryAsync(string name)
    {
        return await expenseCategoryRepository.AddExpenseCategoryAsync(new ExpenseCategory { Name = name });
    }

    public async Task DeleteExpenseCategoryAsync(long id)
    {
        await expenseCategoryRepository.DeleteExpenseCategory(id);
    }

    public async Task<ICollection<ExpenseCategoryResponseDto>> GetAllExpenseCategoriesAsync()
    {
        var allExpenseCategories = await expenseCategoryRepository.SelectAllExpenseCategoriesAsync();
        return allExpenseCategories.Select(x => new ExpenseCategoryResponseDto { Id = x.Id, Name = x.Name }).ToList();
    }

    public async Task<ExpenseCategoryResponseDto> GetExpenseCategoryByNameAync(string name)
    {        
       var expenseCategory = await expenseCategoryRepository.SelectExpenseCategoryByNameAsync(name);
        return new ExpenseCategoryResponseDto()
        {
            Id = expenseCategory.Id,
            Name = expenseCategory.Name
        };
    }

    public Task UpdateExpenseCategoryAsync(ExpenseCategoryResponseDto expenseCategoryResponse, long id)
    {
        throw new NotImplementedException();
    }
}
