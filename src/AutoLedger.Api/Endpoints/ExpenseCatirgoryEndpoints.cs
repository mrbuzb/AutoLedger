using AutoLedger.Application.Services.Interfaces;

namespace AutoLedger.Api.Endpoints;

public static class ExpenseCatirgoryEndpoints
{
    public static void MapExpenseCotegorys(this WebApplication app)
    {
        var userGroup = app.MapGroup("/api/categorys")
                   .WithTags("ExpenseCotegory");

        userGroup.MapGet("/",
        async (IExpenseCategoryService _cotegory) =>
        {
            var cotegories = await _cotegory.GetAllExpenseCategoriesAsync();
            return Results.Ok(cotegories);
        })
        .WithName("GetAllExpenseCategory");

        userGroup.MapGet("/{name}",
            async (string name, IExpenseCategoryService _cotegory) =>
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
