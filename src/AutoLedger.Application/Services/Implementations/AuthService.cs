using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using AutoLedger.Application.Dtos;
using AutoLedger.Application.Helpers;
using AutoLedger.Application.Helpers.Security;
using AutoLedger.Application.Interfaces;
using AutoLedger.Application.Services.Interfaces;
using AutoLedger.Core.Errors;
using AutoLedger.Domain.Entities;
using FluentEmail.Core;
using FluentEmail.Smtp;
using FluentValidation;

namespace AutoLedger.Application.Services.Implementations;

public class AuthService(IRoleRepository _roleRepo, IValidator<UserCreateDto> _validator,
    IUserRepository _userRepo, ITokenService _tokenService, IValidator<UserLoginDto> _validatorForLogin,
    IRefreshTokenRepository _refTokRepo) : IAuthService
{


    public async Task<long> SignUpUserAsync(UserCreateDto userCreateDto)
    {
        var validatorResult = await _validator.ValidateAsync(userCreateDto);
        if (!validatorResult.IsValid)
        {
            string errorMessages = string.Join("; ", validatorResult.Errors.Select(e => e.ErrorMessage));
            throw new AuthException(errorMessages);
        }

        var isEmailExists = await _userRepo.GetUserByEmailAsync(userCreateDto.Email);

        if (isEmailExists == null)
        {

            var tupleFromHasher = PasswordHasher.Hasher(userCreateDto.Password);

            var confirmer = new UserConfirme()
            {
                IsConfirmed = false,
                Email = userCreateDto.Email,
            };


            var user = new User()
            {
                Confirmer = confirmer,
                UserName = userCreateDto.UserName,
                Password = tupleFromHasher.Hash,
                Salt = tupleFromHasher.Salt,
                RoleId = await _roleRepo.GetRoleIdAsync("User")
            };

            long userId = await _userRepo.AddUserAsync(user);

            var foundUser = await _userRepo.GetUserByIdAsync(userId);

            foundUser.Confirmer!.UserId = userId;

            await _userRepo.UpdateUserAsync(foundUser);

            return userId;
        }
        else if (isEmailExists.Confirmer!.IsConfirmed is false)
        {

            var tupleFromHasher = PasswordHasher.Hasher(userCreateDto.Password);

            isEmailExists.UserName = userCreateDto.UserName;
            isEmailExists.Password = tupleFromHasher.Hash;
            isEmailExists.Salt = tupleFromHasher.Salt;
            isEmailExists.RoleId = await _roleRepo.GetRoleIdAsync("User");

            await _userRepo.UpdateUserAsync(isEmailExists);
            return isEmailExists.UserId;
        }

        throw new NotAllowedException("This email already confirmed");
    }


    public async Task<LoginResponseDto> LoginUserAsync(UserLoginDto userLoginDto)
    {
        var resultOfValidator = _validatorForLogin.Validate(userLoginDto);
        if (!resultOfValidator.IsValid)
        {
            string errorMessages = string.Join("; ", resultOfValidator.Errors.Select(e => e.ErrorMessage));
            throw new AuthException(errorMessages);
        }

        var user = await _userRepo.GetUserByUserNameAsync(userLoginDto.UserName);

        var checkUserPassword = PasswordHasher.Verify(userLoginDto.Password, user.Password, user.Salt);

        if (checkUserPassword == false)
        {
            throw new UnauthorizedException("UserName or password incorrect");
        }
        if (user.Confirmer.IsConfirmed == false)
        {
            throw new UnauthorizedException("Email not confirmed");
        }

        var userGetDto = new UserGetDto()
        {
            UserId = user.UserId,
            UserName = user.UserName,
            Email = user.Confirmer.Email,
            Role = user.Role.Name,
        };

        var token = _tokenService.GenerateToken(userGetDto);
        var refreshToken = _tokenService.GenerateRefreshToken();

        var refreshTokenToDB = new RefreshToken()
        {
            Token = refreshToken,
            Expires = DateTime.UtcNow.AddDays(21),
            IsRevoked = false,
            UserId = user.UserId
        };

        await _refTokRepo.AddRefreshToken(refreshTokenToDB);

        var loginResponseDto = new LoginResponseDto()
        {
            AccessToken = token,
            RefreshToken = refreshToken,
            User = new UserGetDto { Email = user.Confirmer.Email, ProfileImgUrl = user.ProfileImgUrl, Role = user.Role.Name, UserId = user.UserId, UserName = user.UserName },
            TokenType = "Bearer",
            Expires = 24
        };


        return loginResponseDto;
    }


    public async Task<LoginResponseDto> RefreshTokenAsync(RefreshRequestDto request)
    {
        ClaimsPrincipal? principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal == null) throw new ForbiddenException("Invalid access token.");


        var userClaim = principal.FindFirst(c => c.Type == "UserId");
        var userId = long.Parse(userClaim.Value);


        var refreshToken = await _refTokRepo.SelectRefreshToken(request.RefreshToken, userId);
        if (refreshToken == null || refreshToken.Expires < DateTime.UtcNow || refreshToken.IsRevoked)
            throw new UnauthorizedException("Invalid or expired refresh token.");

        refreshToken.IsRevoked = true;

        var user = await _userRepo.GetUserByIdAsync(userId);

        var userGetDto = new UserGetDto()
        {
            UserId = user.UserId,
            UserName = user.UserName,
            Email = user.Confirmer!.Email,
            Role = user.Role.Name,
        };

        var newAccessToken = _tokenService.GenerateToken(userGetDto);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        var refreshTokenToDB = new RefreshToken()
        {
            Token = newRefreshToken,
            Expires = DateTime.UtcNow.AddDays(21),
            IsRevoked = false,
            UserId = user.UserId
        };

        await _refTokRepo.AddRefreshToken(refreshTokenToDB);

        return new LoginResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            TokenType = "Bearer",
            Expires = 24
        };
    }

    public async Task LogOut(string token) => await _refTokRepo.DeleteRefreshToken(token);

    public async Task EailCodeSender(string email)
    {
        var user = await _userRepo.GetUserByEmailAsync(email);

        var sender = new SmtpSender(() => new SmtpClient("smtp.gmail.com")
        {
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Port = 587,
            Credentials = new NetworkCredential("qahmadjon11@gmail.com", "nhksnhhxzdbbnqdw")
        });

        Email.DefaultSender = sender;

        var code = Random.Shared.Next(100000, 999999).ToString();

        var sendResponse = await Email
            .From("qahmadjon11@gmail.com")
            .To(email)
            .Subject("Your Confirming Code")
            .Body(code)
            .SendAsync();

        user.Confirmer!.ConfirmingCode = code;
        user.Confirmer.ExpiredDate = DateTime.UtcNow.AddHours(5).AddMinutes(10);
        await _userRepo.UpdateUserAsync(user);
    }

    public async Task<bool> ConfirmCode(string userCode, string email)
    {
        var user = await _userRepo.GetUserByEmailAsync(email);
        var code = user.Confirmer!.ConfirmingCode;
        if (code == null || code != userCode || user.Confirmer.ExpiredDate < DateTime.Now || user.Confirmer.IsConfirmed is true)
        {
            throw new NotAllowedException("Code is incorrect");
        }
        user.Confirmer.IsConfirmed = true;
        await _userRepo.UpdateUserAsync(user);
        return true;
    }
}
