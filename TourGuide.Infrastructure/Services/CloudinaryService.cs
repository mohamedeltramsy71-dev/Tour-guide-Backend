using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using TourGuide.Application.Interfaces;

namespace TourGuide.Infrastructure.Services;

public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IOptions<CloudinarySettings> settings)
    {
        var s = settings.Value;
        var account = new Account(s.CloudName, s.ApiKey, s.ApiSecret);
        _cloudinary = new Cloudinary(account) { Api = { Secure = true } };
    }

    public async Task<string> UploadImageAsync(IFormFile file, string folder)
    {
        using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = folder,
            UseFilename = true,
            UniqueFilename = true,
            Overwrite = false,
            Transformation = new Transformation().Quality("auto").FetchFormat("auto")
        };

        var result = await _cloudinary.UploadAsync(uploadParams);

        if (result.Error is not null)
            throw new Exception($"Cloudinary upload failed: {result.Error.Message}");

        return result.SecureUrl.ToString();
    }

    public async Task DeleteImageAsync(string imageUrl)
    {
        var publicId = ExtractPublicId(imageUrl);
        if (string.IsNullOrEmpty(publicId)) return;

        var deleteParams = new DeletionParams(publicId);
        await _cloudinary.DestroyAsync(deleteParams);
    }

    private static string ExtractPublicId(string imageUrl)
    {
        // https://res.cloudinary.com/cloud/image/upload/v123/folder/filename.jpg
        // => folder/filename
        try
        {
            var uri = new Uri(imageUrl);
            var segments = uri.AbsolutePath.Split('/');
            var uploadIndex = Array.IndexOf(segments, "upload");
            if (uploadIndex < 0) return string.Empty;

            // skip version segment (v123)
            var start = uploadIndex + 2;
            var parts = segments[start..];
            var publicId = string.Join("/", parts);

            // remove extension
            var dotIndex = publicId.LastIndexOf('.');
            return dotIndex > 0 ? publicId[..dotIndex] : publicId;
        }
        catch
        {
            return string.Empty;
        }
    }
}