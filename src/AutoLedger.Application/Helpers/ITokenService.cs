using System.Security.Claims;
using AutoLedger.Application.Dtos;

namespace AutoLedger.Application.Helpers;

public interface ITokenService
{
    public string GenerateToken(UserGetDto user);
    public string GenerateRefreshToken();
    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}






