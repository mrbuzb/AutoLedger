using AutoLedger.Application.Interfaces;
using AutoLedger.Core.Errors;
using AutoLedger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoLedger.Infrastructure.Persistence.Repositories;

public class VehicleRepository(AppDbContext _contect) : IVehicleRepository
{
    public async Task<long> AddVehicleAsync(Vehicle vehicle)
    {
        await _contect.Vehicles.AddAsync(vehicle);
        await _contect.SaveChangesAsync();
        return vehicle.Id;
    }

    public async Task DeleteVehicleAsync(long vehicleId, long userId)
    {
        var vehicle =await GetVehicleByIdAsync(vehicleId);
        if (vehicle == null || vehicle.UserId != userId)
        {
            throw new EntityNotFoundException($"Vehicle not found with id {vehicleId}");
        }
        _contect.Vehicles.Remove(vehicle);
        await _contect.SaveChangesAsync();
    }

    public async Task<ICollection<Vehicle>> GetAllVehicleAsync(long userId)
    {
        return await _contect.Vehicles.Where(x=>x.UserId == userId).ToListAsync();
    }

    public async Task<Vehicle> GetVehicleByIdAsync(long vehicleId)
    {
        var vehicle = await _contect.Vehicles.FirstOrDefaultAsync(x => x.Id == vehicleId);
        if(vehicle == null)
        {
            throw new EntityNotFoundException($"Vehicle not found with id {vehicleId}");
        }
        return vehicle;
    }

    public async Task UpdateVehicleAsync(Vehicle vehicle)
    {
        _contect.Vehicles.Update(vehicle);
        await _contect.SaveChangesAsync();
    }
}
