using AutoLedger.Application.Dtos;

namespace AutoLedger.Application.Services.Interfaces;

public interface IExpenseCategoryService
{
    Task<long> AddExpenseCategoryAsync(string name);
    Task DeleteExpenseCategoryAsync(long id);
    Task<ICollection<ExpenseCategoryResponseDto>> GetAllExpenseCategoriesAsync();
    Task<ExpenseCategoryResponseDto> GetExpenseCategoryByNameAync(string name);
    Task UpdateExpenseCategoryAsync(ExpenseCategoryResponseDto expenseCategoryResponse);
}
