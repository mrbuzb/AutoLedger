using AutoLedger.Application.Dtos;
using AutoLedger.Application.Services.Interfaces;

namespace AutoLedger.Api.Endpoints;

public static class ExpenseEndpoints
{
    public static IEndpointRouteBuilder MapExpenseEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/expenses")
            .WithTags("Expenses");

        group.MapPost("/", async (ExpenseCreateDto dto, IExpenseService service, HttpContext ctx) =>
        {
            var userId = long.Parse(ctx.User.FindFirst("UserId")!.Value);
            var id = await service.AddExpenseAsync(dto, userId);
            return Results.Ok(new { Id = id });
        });

        group.MapPut("/{expenseId:long}", async (long expenseId, ExpenseUpdateDto dto, IExpenseService service, HttpContext ctx) =>
        {
            var userId = long.Parse(ctx.User.FindFirst("UserId")!.Value);
            dto.ExpenseId = expenseId;
            await service.UpdateExpenseAsync(dto, userId);
            return Results.NoContent();
        });

        group.MapDelete("/{expenseId:long}", async (long expenseId, IExpenseService service, HttpContext ctx) =>
        {
            var userId = long.Parse(ctx.User.FindFirst("UserId")!.Value);
            await service.DeleteExpenseAsync(expenseId, userId);
            return Results.NoContent();
        });

        group.MapGet("/{expenseId:long}", async (long expenseId, IExpenseService service) =>
        {
            var expense = await service.GetExpenseByIdAsync(expenseId);
            return Results.Ok(expense);
        });

        group.MapGet("/vehicle/{vehicleId:long}", async (long vehicleId, IExpenseService service) =>
        {
            var expenses = await service.GetExpensesByVehicleIdAsync(vehicleId);
            return Results.Ok(expenses);
        });

        group.MapGet("/vehicle/{vehicleId:long}/category/{categoryId:long}", async (long vehicleId, long categoryId, IExpenseService service) =>
        {
            var expenses = await service.GetExpensesByCategoryAsync(vehicleId, categoryId);
            return Results.Ok(expenses);
        });

        group.MapGet("/vehicle/{vehicleId:long}/daterange", async (long vehicleId, DateTime startDate, DateTime endDate, IExpenseService service) =>
        {
            var expenses = await service.GetExpensesByDateRangeAsync(vehicleId, startDate, endDate);
            return Results.Ok(expenses);
        });

        group.MapGet("/vehicle/{vehicleId:long}/latest", async (long vehicleId, int count, IExpenseService service) =>
        {
            var expenses = await service.GetLatestExpensesAsync(vehicleId, count);
            return Results.Ok(expenses);
        });

        group.MapGet("/vehicle/{vehicleId:long}/total", async (long vehicleId, DateTime startDate, DateTime endDate, IExpenseService service) =>
        {
            var total = await service.GetTotalAmountAsync(vehicleId, startDate, endDate);
            return Results.Ok(new { TotalAmount = total });
        });

        return endpoints;
    }
}
