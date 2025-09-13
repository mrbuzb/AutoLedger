using AutoLedger.Application.Interfaces;
using AutoLedger.Core.Errors;
using AutoLedger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoLedger.Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext _context) : IUserRepository
{
    public async Task<long> AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user.UserId;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        var user = await _context.Users.Include(_ => _.Confirmer).Include(x => x.Role).FirstOrDefaultAsync(x => x.Confirmer!.Email == email);
        return user;
    }

    public async Task<long?> CheckEmailExistsAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(_ => _.Confirmer!.Email == email);
        if (user is null)
        {
            return null;
        }
        return user.UserId;
    }


    public async Task DeleteUserByIdAsync(long userId)
    {
        var user = await GetUserByIdAsync(userId);
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User> GetUserByIdAsync(long id)
    {
        var user = await _context.Users.Include(_ => _.Confirmer).Include(_ => _.Role).FirstOrDefaultAsync(x => x.UserId == id);
        if (user == null)
        {
            throw new EntityNotFoundException($"Entity with {id} not found");
        }
        return user;
    }

    public async Task<User> GetUserByUserNameAsync(string userName)
    {
        var user = await _context.Users.Include(_ => _.Confirmer).Include(_ => _.Role).FirstOrDefaultAsync(x => x.UserName == userName);
        if (user == null)
        {
            throw new EntityNotFoundException($"Entity with {userName} not found");
        }
        return user;
    }

    public async Task AddConfirmer(UserConfirme confirmer)
    {
        await _context.Confirmers.AddAsync(confirmer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserRoleAsync(long userId, string userRole)
    {
        var user = await GetUserByIdAsync(userId);
        var role = await _context.UserRoles.FirstOrDefaultAsync(x => x.Name == userRole);
        if (role == null)
        {
            throw new EntityNotFoundException($"Role : {userRole} not found");
        }
        user.RoleId = role.Id;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User> GetUserByGoogleId(string googleId)
    {
        return await _context.Users.Include(x=>x.Confirmer).FirstOrDefaultAsync(x=>x.GoogleId == googleId);
    }
}
