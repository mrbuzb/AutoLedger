using AutoLedger.Domain.Entities;
using Microsoft.IdentityModel.Abstractions;

namespace AutoLedger.Application.Interfaces;

public interface IVehicleRepository
{
    Task<long> AddVehicleAsync(Vehicle vehicle);
    Task DeleteVehicleAsync(long vehicleId,long userId);
    Task UpdateVehicleAsync(Vehicle vehicle);
    Task<Vehicle> GetVehicleByIdAsync(long vehicleId);
    Task<ICollection<Vehicle>> GetAllVehicleAsync(long userId);
}
