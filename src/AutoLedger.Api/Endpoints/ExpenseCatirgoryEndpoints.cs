using AutoLedger.Application.Dtos;
using AutoLedger.Application.Services.Interfaces;

namespace AutoLedger.Api.Endpoints;

public static class ExpenseCatirgoryEndpoints
{
    public static void MapExpenseCotegorys(this WebApplication app)
    {
        var userGroup = app.MapGroup("/api/categorys")
                   .WithTags("ExpenseCotegory");

        userGroup.MapGet("/get-all",
        async (IExpenseCategoryService _cotegory) =>
        {
            var cotegories = await _cotegory.GetAllExpenseCategoriesAsync();
            return Results.Ok(cotegories);
        })
        .WithName("GetAllExpenseCategory");
        userGroup.MapPost("/create",
            async (string name, IExpenseCategoryService _cotegory) =>
        {
            var cotegory = await _cotegory.AddExpenseCategoryAsync(name);
            return Results.Ok(cotegory);
        })
            .WithName("CreatExpenseCategory");
        userGroup.MapDelete("/delete",
            async (long id, IExpenseCategoryService _cotegory) =>
        {
            await _cotegory.DeleteExpenseCategoryAsync(id);
            return Results.Ok();
        })
        .WithName("deleteExpenseCategory");
        userGroup.MapPut("/update",
           async (ExpenseCategoryResponseDto dto, IExpenseCategoryService _cotegory) =>
           {
               await _cotegory.UpdateExpenseCategoryAsync(dto);
               return Results.Ok();
           })
           .WithName("updateExpenseCategory");
        userGroup.MapGet("/getByName",
            async(string name, IExpenseCategoryService _cotegory) =>
            {
                var cotegory = await _cotegory.GetExpenseCategoryByNameAync(name);
                if (cotegory == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(cotegory);
            })
            .WithName("getByNameExpenseCategory");




    }
}
