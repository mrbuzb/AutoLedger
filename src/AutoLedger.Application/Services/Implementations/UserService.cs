using AutoLedger.Application.Interfaces;
using AutoLedger.Application.Services.Interfaces;
using AutoLedger.Core.Errors;
using Microsoft.AspNetCore.Http;

namespace AutoLedger.Application.Services.Implementations;

public class UserService(IUserRepository _userRepository, ICloudService _cloudService) : IUserService
{
    public async Task DeleteUserByIdAsync(long userId, string userRole)
    {
        if (userRole == "SuperAdmin")
        {
            await _userRepository.DeleteUserByIdAsync(userId);
        }
        else if (userRole == "Admin")
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user.Role.Name == "User")
            {
                await _userRepository.DeleteUserByIdAsync(userId);
            }
            else
            {
                throw new NotAllowedException("Admin can not delete Admin or SuperAdmin");
            }
        }
    }

    public async Task UploadProfileImgAsync(IFormFile file, long userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);

        user.ProfileImgUrl = await _cloudService.UploadProfileImageAsync(file);
        await _userRepository.UpdateUserAsync(user);
    }

    public async Task UpdateUserRoleAsync(long userId, string userRole)
    {
        await _userRepository.UpdateUserRoleAsync(userId, userRole);
    }
}
