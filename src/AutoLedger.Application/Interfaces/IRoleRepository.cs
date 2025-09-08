using AutoLedger.Domain.Entities;

namespace AutoLedger.Application.Interfaces;

public interface IRoleRepository
{
    Task<ICollection<User>> GetAllUsersByRoleAsync(string role);
    Task<List<UserRole>> GetAllRolesAsync();
    Task<long> GetRoleIdAsync(string role);
}