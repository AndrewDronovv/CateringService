using CateringService.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace CateringService.Application.Services
{
    public sealed class LocalFileStorageService : IImageStorageService
    {
        private readonly IWebHostEnvironment _environment;

        public LocalFileStorageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> SaveFileAsync(IFormFile imageFile, string[] allowedFileExtensions)
        {
            if (imageFile is null)
                throw new ArgumentNullException(nameof(imageFile));

            if (imageFile.Length > 1 * 1024 * 1024)
                throw new ArgumentException("File size should not exceed 1 MB", nameof(imageFile));

            var ext = Path.GetExtension(imageFile.FileName)
                          .ToLowerInvariant();

            if (!allowedFileExtensions.Contains(ext))
                throw new ArgumentException(
                    $"Only these extensions are allowed: {string.Join(", ", allowedFileExtensions)}",
                    nameof(imageFile));

            var uploadsPath = Path.Combine(_environment.ContentRootPath, "Uploads");

            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsPath, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await imageFile.CopyToAsync(stream);

            return fileName;
        }

        public void DeleteFile(string fileNameWithExtension)
        {
            if (string.IsNullOrEmpty(fileNameWithExtension))
                throw new ArgumentNullException(nameof(fileNameWithExtension));

            var fullPath = Path.Combine(_environment.ContentRootPath, "Uploads", fileNameWithExtension);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException("File not found.", fullPath);

            File.Delete(fullPath);
        }
    }
}