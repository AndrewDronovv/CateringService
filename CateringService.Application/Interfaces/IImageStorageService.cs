using Microsoft.AspNetCore.Http;

namespace CateringService.Application.Interfaces;

public interface IImageStorageService
{
    Task<string> SaveFileAsync(IFormFile imageFile, string[] allowedFileExtensions);
    void DeleteFile(string fileNameWithExtension);
}