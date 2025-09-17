using AutoLedger.Application.Dtos;
using AutoLedger.Application.Interfaces;
using AutoLedger.Application.Services.Interfaces;
using AutoLedger.Core.Errors;
using AutoLedger.Domain.Entities;

namespace AutoLedger.Application.Services.Implementations;

public class VehicleService(IVehicleRepository _repo) : IVehicleService
{
    public async Task<long> AddVehicleAsync(VehicleCreateDto vehicle, long userId)
    {
        return await _repo.AddVehicleAsync(new Vehicle
        {
            CreatedAt = DateTime.Now,
            Year = vehicle.Year,
            FuelType = vehicle.FuelType,
            Model = vehicle.Model,
            Plate = vehicle.Plate,
            Make = vehicle.Make,
            UserId = userId,
        });
    }

    public async Task DeleteVehicleAsync(long vehicleId, long userId)
    {
        await _repo.DeleteVehicleAsync(vehicleId, userId);
    }

    public async Task<ICollection<VehicleDto>> GetAllVehicleAsync(long userId)
    {
        var vehicles = await _repo.GetAllVehicleAsync(userId);
        return vehicles.Select(Converter).ToList();
    }

    public async Task<VehicleDto> GetVehicleByIdAsync(long vehicleId)
    {
        return Converter(await _repo.GetVehicleByIdAsync(vehicleId));
    }

    public async Task UpdateVehicleAsync(VehicleUpdateDto vehicle, long userId)
    {
        var entity = await _repo.GetVehicleByIdAsync(vehicle.Id);
        if (entity.UserId != userId)
        {
            throw new NotAllowedException($"You don't have an access for update another persons car");
        }
        entity.FuelType = vehicle.FuelType;
        entity.Model = vehicle.Model;
        entity.Plate = vehicle.Plate;
        entity.Make = vehicle.Make;
        entity.Year = vehicle.Year;
        await _repo.UpdateVehicleAsync(entity);
    }

    private VehicleDto Converter(Vehicle entity)
    {
        return new VehicleDto
        {
            CreatedAt = entity.CreatedAt,
            FuelType = entity.FuelType,
            Id = entity.Id,
            Model = entity.Model,
            Plate = entity.Plate,
            Make = entity.Make,
            Year = entity.Year,
        };
    }
}
