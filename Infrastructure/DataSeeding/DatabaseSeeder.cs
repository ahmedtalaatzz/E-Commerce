using Domain.Entities;
using e_commerce.core.Domain.Entities;
using e_commerce.infrastructure.DbContxt;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.DataSeeding
{
    /// <summary>
    /// Database seeder that reads images from wwwroot and creates database records
    /// </summary>
    public class DatabaseSeeder : IDataSeeder
    {
        private readonly ApplicationDbContexts _context;
        private readonly ILogger<DatabaseSeeder> _logger;
        private readonly string _wwwrootPath;

        public DatabaseSeeder(ApplicationDbContexts context, ILogger<DatabaseSeeder> logger)
        {
            _context = context;
            _logger = logger;
            // Get wwwroot path relative to current directory
            _wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        }

        /// <summary>
        /// Seeds all entities from their respective image folders
        /// </summary>
        public async Task SeedAsync()
        {
            try
            {
                _logger.LogInformation("Starting database seeding...");

                await SeedBannersAsync();
                await SeedBrandsAsync();
                await SeedPartnersAsync();

                _logger.LogInformation("Database seeding completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during database seeding");
                throw;
            }
        }

        /// <summary>
        /// Seeds banners from wwwroot/uploads/banners folder
        /// </summary>
        private async Task SeedBannersAsync()
        {
            var bannerFolder = Path.Combine(_wwwrootPath, "uploads", "banners");

            if (!Directory.Exists(bannerFolder))
            {
                _logger.LogWarning("Banner folder does not exist: {Path}", bannerFolder);
                return;
            }

            var imageFiles = Directory.GetFiles(bannerFolder, "*.*")
                .Where(f => IsImageFile(f))
                .ToList();

            if (!imageFiles.Any())
            {
                _logger.LogInformation("No banner images found in {Path}", bannerFolder);
                return;
            }

            _logger.LogInformation("Found {Count} banner images", imageFiles.Count);

            foreach (var imageFile in imageFiles)
            {
                var fileName = Path.GetFileName(imageFile);
                var relativePath = $"/uploads/banners/{fileName}";

                // Check if banner already exists
                var exists = await _context.Banners
                    .AnyAsync(b => b.ImageUrl == relativePath && !b.IsDeleted);

                if (!exists)
                {
                    var banner = new Banner
                    {
                        ImageUrl = relativePath,
                        IsActive = true
                    };

                    _context.Banners.Add(banner);
                    _logger.LogInformation("Added banner: {Path}", relativePath);
                }
                else
                {
                    _logger.LogDebug("Banner already exists: {Path}", relativePath);
                }
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Banner seeding completed");
        }

        /// <summary>
        /// Seeds brands from wwwroot/uploads/brands folder
        /// </summary>
        private async Task SeedBrandsAsync()
        {
            var brandFolder = Path.Combine(_wwwrootPath, "uploads", "brands");

            if (!Directory.Exists(brandFolder))
            {
                _logger.LogWarning("Brand folder does not exist: {Path}", brandFolder);
                return;
            }

            var imageFiles = Directory.GetFiles(brandFolder, "*.*")
                .Where(f => IsImageFile(f))
                .ToList();

            if (!imageFiles.Any())
            {
                _logger.LogInformation("No brand images found in {Path}", brandFolder);
                return;
            }

            _logger.LogInformation("Found {Count} brand images", imageFiles.Count);

            foreach (var imageFile in imageFiles)
            {
                var fileName = Path.GetFileName(imageFile);
                var nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                var relativePath = $"/uploads/brands/{fileName}";

                // Check if brand already exists
                var exists = await _context.Brands
                    .AnyAsync(b => b.ImageUrl == relativePath && !b.IsDeleted);

                if (!exists)
                {
                    // Parse filename format: "Samsung - سامسونج.png"
                    var names = ParseBillingualName(nameWithoutExtension);

                    if (names == null)
                    {
                        _logger.LogWarning("Skipping brand with invalid filename format: {FileName}. Expected format: 'EnglishName - ArabicName'", fileName);
                        continue;
                    }

                    var brand = new Brand
                    {
                        NameAr = names.Value.Arabic,
                        NameEn = names.Value.English,
                        ImageUrl = relativePath,
                        IsActive = true
                    };

                    _context.Brands.Add(brand);
                    _logger.LogInformation("Added brand: {NameEn} ({NameAr}) - {Path}", names.Value.English, names.Value.Arabic, relativePath);
                }
                else
                {
                    _logger.LogDebug("Brand already exists: {Path}", relativePath);
                }
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Brand seeding completed");
        }

        /// <summary>
        /// Seeds partners from wwwroot/uploads/partners folder
        /// </summary>
        private async Task SeedPartnersAsync()
        {
            var partnerFolder = Path.Combine(_wwwrootPath, "uploads", "partners");

            if (!Directory.Exists(partnerFolder))
            {
                _logger.LogWarning("Partner folder does not exist: {Path}", partnerFolder);
                return;
            }

            var imageFiles = Directory.GetFiles(partnerFolder, "*.*")
                .Where(f => IsImageFile(f))
                .ToList();

            if (!imageFiles.Any())
            {
                _logger.LogInformation("No partner images found in {Path}", partnerFolder);
                return;
            }

            _logger.LogInformation("Found {Count} partner images", imageFiles.Count);

            foreach (var imageFile in imageFiles)
            {
                var fileName = Path.GetFileName(imageFile);
                var nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                var relativePath = $"/uploads/partners/{fileName}";

                // Check if partner already exists
                var exists = await _context.Partners
                    .AnyAsync(p => p.ImageUrl == relativePath && !p.IsDeleted);

                if (!exists)
                {
                    // Parse filename format: "Nike - نايكي.png"
                    var names = ParseBillingualName(nameWithoutExtension);

                    if (names == null)
                    {
                        _logger.LogWarning("Skipping partner with invalid filename format: {FileName}. Expected format: 'EnglishName - ArabicName'", fileName);
                        continue;
                    }

                    var partner = new Partner
                    {
                        NameAr = names.Value.Arabic,
                        NameEn = names.Value.English,
                        ImageUrl = relativePath,
                        IsActive = true
                    };

                    _context.Partners.Add(partner);
                    _logger.LogInformation("Added partner: {NameEn} ({NameAr}) - {Path}", names.Value.English, names.Value.Arabic, relativePath);
                }
                else
                {
                    _logger.LogDebug("Partner already exists: {Path}", relativePath);
                }
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Partner seeding completed");
        }

        /// <summary>
        /// Checks if file is an image based on extension
        /// </summary>
        private bool IsImageFile(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".webp";
        }

        /// <summary>
        /// Parses bilingual filename format: "EnglishName - ArabicName"
        /// </summary>
        /// <returns>Tuple with English and Arabic names, or null if format is invalid</returns>
        private (string English, string Arabic)? ParseBillingualName(string nameWithoutExtension)
        {
            // Expected format: "Samsung - سامسونج" or "Nike - نايكي"
            var separator = " - ";

            if (!nameWithoutExtension.Contains(separator))
            {
                return null;
            }

            var parts = nameWithoutExtension.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
            {
                return null;
            }

            var englishName = parts[0].Trim();
            var arabicName = parts[1].Trim();

            if (string.IsNullOrWhiteSpace(englishName) || string.IsNullOrWhiteSpace(arabicName))
            {
                return null;
            }

            return (englishName, arabicName);
        }
    }
}
