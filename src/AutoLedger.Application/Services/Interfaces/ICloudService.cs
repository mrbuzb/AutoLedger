using Microsoft.AspNetCore.Http;

namespace AutoLedger.Application.Services.Interfaces;

public interface ICloudService
{
    Task<string> UploadProfileImageAsync(IFormFile file);
    Task<string> UploadTrackAsync(IFormFile file);
}
