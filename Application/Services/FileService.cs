using e_commerce.core.ServiceContracts;
using Microsoft.AspNetCore.Http;

namespace e_commerce.core.Services
{
    /// <summary>
    /// Service for handling file uploads and deletions
    /// </summary>
    public class FileService : IFileService
    {
        private readonly string _basePath;
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private readonly long _maxFileSize = 10 * 1024 * 1024; // 10MB

        public FileService()
        {
            _basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(_basePath))
                Directory.CreateDirectory(_basePath);
        }

        public async Task<string?> UploadFileAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                return null;

            if (file.Length > _maxFileSize)
                throw new InvalidOperationException("File size exceeds 10MB limit");

            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!_allowedExtensions.Contains(extension))
                throw new InvalidOperationException($"File type {extension} is not allowed. Allowed types: jpg, jpeg, png, webp");

            return await SaveFileAsync(file, folder);
        }

        private async Task<string?> SaveFileAsync(IFormFile file, string folder)
        {
            var folderPath = Path.Combine(_basePath, folder);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName).ToLower()}";
            var filePath = Path.Combine(folderPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/uploads/{folder}/{fileName}";
        }

        public Task<bool> DeleteFileAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return Task.FromResult(false);

            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath.TrimStart('/'));

            if (!File.Exists(fullPath))
                return Task.FromResult(false);

            File.Delete(fullPath);
            return Task.FromResult(true);
        }
    }
}
