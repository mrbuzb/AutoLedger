using AutoLedger.Application.Services.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace AutoLedger.Application.Services.Implementations;

public class CloudinaryService : ICloudService
{
    private readonly Cloudinary _cloudinary;
    public CloudinaryService(IConfiguration configuration)
    {
        var cloudName = configuration["Cloudinary:CloudName"];
        var apiKey = configuration["Cloudinary:ApiKey"];
        var apiSecret = configuration["Cloudinary:ApiSecret"];

        var account = new Account(cloudName, apiKey, apiSecret);
        _cloudinary = new Cloudinary(account);
    }

    public async Task<string> UploadProfileImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Rasm fayli bo‘lishi shart.");

        if (!IsImage(file))
            throw new ArgumentException("Faqat rasm fayllar qabul qilinadi.");

        if (file.Length > 5 * 1024 * 1024)
            throw new ArgumentException("Rasm hajmi 5MB dan oshmasligi kerak.");

        await using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(file.FileName, stream),
            Folder = "photos"
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new Exception("Cloudinary upload failed: " + uploadResult.Error?.Message);
        }

        return uploadResult.SecureUrl.ToString();
    }

    public async Task<string> UploadTrackAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Musiqa fayli bo‘lishi shart.");

        if (!IsAudio(file))
            throw new ArgumentException("Faqat musiqiy fayllar qabul qilinadi (mp3, wav, flac).");

        if (file.Length > 15 * 1024 * 1024) 
            throw new ArgumentException("Musiqa fayli hajmi 15MB dan oshmasligi kerak.");

        await using var stream = file.OpenReadStream();

        var uploadParams = new RawUploadParams()
        {
            File = new FileDescription(file.FileName, stream),
            Folder = "tracks",
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new Exception("Cloudinary upload failed: " + uploadResult.Error?.Message);
        }

        return uploadResult.SecureUrl.ToString();
    }




    private bool IsImage(IFormFile file)
    {
        var allowedHeaders = new Dictionary<string, byte[]>
        {
            { "jpeg", new byte[] { 0xFF, 0xD8, 0xFF } },
            { "png", new byte[] { 0x89, 0x50, 0x4E, 0x47 } },
            { "gif", new byte[] { 0x47, 0x49, 0x46, 0x38 } },
            { "bmp", new byte[] { 0x42, 0x4D } },
            { "webp", new byte[] { 0x52, 0x49, 0x46, 0x46 } }
        };

        using var reader = new BinaryReader(file.OpenReadStream());
        var fileHeader = reader.ReadBytes(4);

        return allowedHeaders.Any(h => fileHeader.Take(h.Value.Length).SequenceEqual(h.Value));
    }

    private bool IsAudio(IFormFile file)
    {
        var permittedExtensions = new[] { ".mp3", ".wav", ".flac", ".aac", ".ogg" };
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        return !string.IsNullOrEmpty(ext) && permittedExtensions.Contains(ext);
    }
}
