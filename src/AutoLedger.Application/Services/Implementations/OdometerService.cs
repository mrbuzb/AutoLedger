using AutoLedger.Application.Dtos;
using AutoLedger.Application.Interfaces;
using AutoLedger.Application.Services.Interfaces;
using AutoLedger.Core.Errors;
using AutoLedger.Domain.Entities;

namespace AutoLedger.Application.Services.Implementations;

public class OdometerService(IOdometerRepository _repo) : IOdometerService
{
    public async Task<long> AddOdometerAsync(OdometerCreateDto odometer, long userId)
    {
        return await _repo.AddOdometerAsync(new Odometer
        {
            Date = odometer.Date,
            Value = odometer.Value,
            VehicleId = odometer.VehicleId,
        });
    }

    public async Task DeleteOdometerAsync(long odometerId, long userId)
    {
        await _repo.DeleteOdometerAsync(odometerId, userId);
    }

    public async Task<OdometerResponseDto> GetOdometerByIdAsync(long odometerId)
    {
        return Converter(await _repo.GetOdometerByIdAsync(odometerId));
    }

    public async Task<ICollection<OdometerResponseDto>> GetOdometersByVehicleIdAsync(long vehicleId)
    {
        var odometers = await _repo.GetOdometersByVehicleIdAsync(vehicleId);
        return odometers.Select(Converter).ToList();
    }

    public async Task UpdateOdometerAsync(OdometerUpdateDto odometer, long userId)
    {
        var entity = await _repo.GetOdometerByIdAsync(odometer.OdometerId);
        var checkUser = await _repo.OdometerOwnerAsync(odometer.OdometerId,userId);
        if (!checkUser)
        {
            throw new NotAllowedException();
        }
        entity.Value = odometer.Value;
        entity.Date = odometer.Date;
        await _repo.UpdateOdometerAsync(entity);
    }
    private OdometerResponseDto Converter(Odometer odometer)
    {
        return new OdometerResponseDto
        {
            Date = odometer.Date,
            Id = odometer.Id,
            Value = odometer.Value,
            VehicleId = odometer.VehicleId,
        };
    }
}
