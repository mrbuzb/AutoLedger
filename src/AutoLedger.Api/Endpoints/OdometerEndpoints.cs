using AutoLedger.Application.Dtos;
using AutoLedger.Application.Services.Interfaces;

namespace AutoLedger.Api.Endpoints;

public static class OdometerEndpoints
{
    public static IEndpointRouteBuilder MapOdometerEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/odometer")
            .WithTags("Odometers")
            .RequireAuthorization(); 

        group.MapPost("/", async (OdometerCreateDto dto, IOdometerService service, HttpContext ctx) =>
        {
            var userId = long.Parse(ctx.User.FindFirst("UserId")!.Value);
            var id = await service.AddOdometerAsync(dto, userId);
            return Results.Ok(new { Id = id });
        });

        group.MapPut("/", async (OdometerUpdateDto dto, IOdometerService service, HttpContext ctx) =>
        {
            var userId = long.Parse(ctx.User.FindFirst("UserId")!.Value);
            await service.UpdateOdometerAsync(dto, userId);
            return Results.NoContent();
        });

        group.MapDelete("/{Id}", async (long odometerId, IOdometerService service, HttpContext ctx) =>
        {
            var userId = long.Parse(ctx.User.FindFirst("UserId")!.Value);
            await service.DeleteOdometerAsync(odometerId, userId);
            return Results.NoContent();
        });

        group.MapGet("/{Id}", async (long odometerId, IOdometerService service) =>
        {
            var odometer = await service.GetOdometerByIdAsync(odometerId);
            return Results.Ok(odometer);
        });

        group.MapGet("/{vehicleId:long}", async (long vehicleId, IOdometerService service) =>
        {
            var odometers = await service.GetOdometersByVehicleIdAsync(vehicleId);
            return Results.Ok(odometers);
        });

        return endpoints;
    }
}
