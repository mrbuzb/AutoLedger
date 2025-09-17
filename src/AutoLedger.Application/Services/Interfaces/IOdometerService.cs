using AutoLedger.Application.Dtos;

namespace AutoLedger.Application.Services.Interfaces;

public interface IOdometerService
{
    Task<long> AddOdometerAsync(OdometerCreateDto odometer, long userId);
    Task DeleteOdometerAsync(long odometerId, long userId);
    Task UpdateOdometerAsync(OdometerUpdateDto odometer, long userId);
    Task<ICollection<OdometerResponseDto>> GetOdometersByVehicleIdAsync(long vehicleId);
    Task<OdometerResponseDto> GetOdometerByIdAsync(long odometerId);
}
