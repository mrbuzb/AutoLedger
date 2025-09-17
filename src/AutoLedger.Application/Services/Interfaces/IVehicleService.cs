using AutoLedger.Application.Dtos;

namespace AutoLedger.Application.Services.Interfaces;

public interface IVehicleService
{
    Task<long> AddVehicleAsync(VehicleCreateDto vehicle,long userId);
    Task DeleteVehicleAsync(long vehicleId, long userId);
    Task UpdateVehicleAsync(VehicleUpdateDto vehicle,long userId);
    Task<VehicleDto> GetVehicleByIdAsync(long vehicleId);
    Task<ICollection<VehicleDto>> GetAllVehicleAsync(long userId);
}
