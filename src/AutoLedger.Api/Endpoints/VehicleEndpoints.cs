using AutoLedger.Application.Dtos;
using AutoLedger.Application.Services.Interfaces;

namespace AutoLedger.Api.Endpoints;

public static class VehicleEndpoints
{
    public static IEndpointRouteBuilder MapVehicleEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/vehicles")
            .WithTags("Vehicles");
            

        group.MapPost("/", async (VehicleCreateDto dto, IVehicleService service, HttpContext ctx) =>
        {
            var userId = long.Parse(ctx.User.FindFirst("UserId")!.Value);
            var id = await service.AddVehicleAsync(dto, userId);
            return Results.Ok(new { Id = id });
        });

        group.MapPut("/{vehicleId:long}", async (long vehicleId, VehicleUpdateDto dto, IVehicleService service, HttpContext ctx) =>
        {
            var userId = long.Parse(ctx.User.FindFirst("UserId")!.Value);
            dto.Id = vehicleId;
            await service.UpdateVehicleAsync(dto, userId);
            return Results.NoContent();
        });

        group.MapDelete("/{vehicleId:long}", async (long vehicleId, IVehicleService service, HttpContext ctx) =>
        {
            var userId = long.Parse(ctx.User.FindFirst("UserId")!.Value);
            await service.DeleteVehicleAsync(vehicleId, userId);
            return Results.NoContent();
        });

        group.MapGet("/{vehicleId:long}", async (long vehicleId, IVehicleService service) =>
        {
            var vehicle = await service.GetVehicleByIdAsync(vehicleId);
            return Results.Ok(vehicle);
        });

        group.MapGet("/", async (IVehicleService service, HttpContext ctx) =>
        {
            var userId = long.Parse(ctx.User.FindFirst("UserId")!.Value);
            var vehicles = await service.GetAllVehicleAsync(userId);
            return Results.Ok(vehicles);
        });

        return endpoints;
    }
}
