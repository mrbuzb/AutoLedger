using AutoLedger.Application.Interfaces;
using AutoLedger.Core.Errors;
using AutoLedger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoLedger.Infrastructure.Persistence.Repositories;

public class OdometerRepository(AppDbContext _context) : IOdometerRepository
{
    public async Task<long> AddOdometerAsync(Odometer odometer)
    {
        await _context.Odometers.AddAsync(odometer);
        await _context.SaveChangesAsync();
        return odometer.Id;
    }

    public async Task DeleteOdometerAsync(long odometerId, long userId)
    {
        var odometer = await _context.Odometers.Include(x=>x.Vehicle).FirstOrDefaultAsync(x=>x.Id == odometerId);
        if(odometer == null || odometer.Vehicle.UserId != userId)
        {
            throw new NotAllowedException();
        }
        _context.Odometers.Remove(odometer);
        await _context.SaveChangesAsync();
    }

    public async Task<Odometer> GetOdometerByIdAsync(long odometerId)
    {
        var odometer = await _context.Odometers.FindAsync(odometerId);
        if(odometer == null)
        {
            throw new EntityNotFoundException($"Odometer not found with id {odometerId}");
        }
        return odometer;
    }

    public async Task<ICollection<Odometer>> GetOdometersByVehicleIdAsync(long vehicleId)
    {
        return await _context.Odometers.Where(x=>x.VehicleId == vehicleId).ToListAsync();
    }

    public async Task<bool> OdometerOwnerAsync(long odometerId, long userId)
    {
        return await _context.Odometers.AnyAsync(x=>x.Id == odometerId && x.Vehicle.UserId == userId);
    }

    public async Task UpdateOdometerAsync(Odometer odometer)
    {
        _context.Odometers.Update(odometer);
        await _context.SaveChangesAsync();
    }
}
