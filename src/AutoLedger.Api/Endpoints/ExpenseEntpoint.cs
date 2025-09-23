using AutoLedger.Application.Dtos;
using AutoLedger.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AutoLedger.Api.Endpoints;

public static class ExpenseEndpoints
{
    public record DateRange(long vehicleId, DateTime startDate, DateTime endDate);
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

        group.MapPut("/", async (long expenseId, ExpenseUpdateDto dto, IExpenseService service, HttpContext ctx) =>
        {
            var userId = long.Parse(ctx.User.FindFirst("UserId")!.Value);
            dto.ExpenseId = expenseId;
            await service.UpdateExpenseAsync(dto, userId);
            return Results.NoContent();
        });

        group.MapDelete("/{Id}", async (long expenseId, IExpenseService service, HttpContext ctx) =>
        {
            var userId = long.Parse(ctx.User.FindFirst("UserId")!.Value);
            await service.DeleteExpenseAsync(expenseId, userId);
            return Results.NoContent();
        });

        group.MapGet("/{Id}", async (long expenseId, IExpenseService service) =>
        {
            var expense = await service.GetExpenseByIdAsync(expenseId);
            return Results.Ok(expense);
        });

        group.MapGet("/by{vehicleId}", async (long vehicleId, IExpenseService service) =>
        {
            var expenses = await service.GetExpensesByVehicleIdAsync(vehicleId);
            return Results.Ok(expenses);
        });

        group.MapGet("/by{vehicleId}/{categoryId}", async (long vehicleId, long categoryId, IExpenseService service) =>
        {
            var expenses = await service.GetExpensesByCategoryAsync(vehicleId, categoryId);
            return Results.Ok(expenses);
        });

        group.MapPost("/{vehicleId}/daterange", async ([FromBody]DateRange record,[FromServices] IExpenseService service) =>
        {
            var expenses = await service.GetExpensesByDateRangeAsync(record.vehicleId, record.startDate, record.endDate);
            return Results.Ok(expenses);
        });

        group.MapGet("/{vehicleId}/latest", async (long vehicleId, int count, IExpenseService service) =>
        {
            var expenses = await service.GetLatestExpensesAsync(vehicleId, count);
            return Results.Ok(expenses);
        });

        group.MapPost("/{vehicleId}/total", async ([FromBody]DateRange record, [FromServices]IExpenseService service) =>
        {
            var total = await service.GetTotalAmountAsync(record.vehicleId, record.startDate, record.endDate);
            return Results.Ok(new { TotalAmount = total });
        });

        return endpoints;
    }
}
