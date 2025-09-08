using AutoLedger.Application.Dtos;

namespace AutoLedger.Application.Services.Interfaces;

public interface IRoleService
{
    Task<ICollection<UserGetDto>> GetAllUsersByRoleAsync(string role);
    Task<List<RoleGetDto>> GetAllRolesAsync();
    Task<long> GetRoleIdAsync(string role);
}