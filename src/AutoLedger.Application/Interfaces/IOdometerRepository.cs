using AutoLedger.Domain.Entities;

namespace AutoLedger.Application.Interfaces;

public interface IOdometerRepository
{
    Task<long> AddOdometerAsync(Odometer odometer);
    Task DeleteOdometerAsync(long odometerId, long userId);
    Task UpdateOdometerAsync(Odometer odometer);
    Task<ICollection<Odometer>> GetOdometersByVehicleIdAsync(long vehicleId);
    Task<Odometer> GetOdometerByIdAsync(long odometerId);
}
