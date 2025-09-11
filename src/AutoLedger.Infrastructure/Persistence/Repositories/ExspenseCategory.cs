using AutoLedger.Application.Interfaces;
using AutoLedger.Core.Errors;
using AutoLedger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoLedger.Infrastructure.Persistence.Repositories
{
    public class ExspenseCategory(AppDbContext appDbContext) : IExspenseCategory
    {
        public async Task<long> AddExpenseCategoryAsync(ExpenseCategory expenseCategory)
        {
            await appDbContext.ExpenseCategories.AddAsync(expenseCategory);
            await appDbContext.SaveChangesAsync();
            return expenseCategory.Id;
        }

        public async Task DeleteExpenseCategory(long id)
        {
            var expenseCategory = await appDbContext.ExpenseCategories.FindAsync(id);
            if (expenseCategory is null)
            {
                throw new EntityNotFoundException($"category : {id} not found");
            }
            appDbContext.ExpenseCategories.Remove(expenseCategory);
            await appDbContext.SaveChangesAsync();
        }

        public async Task<ICollection<ExpenseCategory>> SelectAllExpenseCategoriesAsync()
        {
            return await appDbContext.ExpenseCategories.ToListAsync();
        }

        public async Task<ExpenseCategory?> SelectExpenseCategoryByIdAsync(long id)
        {
           return await appDbContext.ExpenseCategories.FindAsync(id);
        }

        public async Task<ExpenseCategory?> SelectExpenseCategoryByNameAsync(string name)
        {
            return await appDbContext.ExpenseCategories.
                FirstOrDefaultAsync(e => e.Name.ToLower() == name.ToLower());
        }

        public async Task UpdateExpenseCategoryAsync(ExpenseCategory expenseCategory)
        {
            appDbContext.ExpenseCategories.Update(expenseCategory);
             await appDbContext.SaveChangesAsync();
        }
    }
}
