# Database Seeding Implementation

## Overview

Automatic database seeding implementation that reads existing images from `wwwroot/uploads` folders and creates corresponding database records on application startup.

## Features

✅ **Automatic Seeding on Startup** - Runs every time the application starts
✅ **Idempotent** - Safe to run multiple times, avoids duplicates
✅ **Smart Name Translation** - Detects Arabic/English filenames and translates accordingly
✅ **Clean Architecture** - Follows existing project structure
✅ **Production Ready** - Includes logging and error handling

## How It Works

### Folder Structure Expected

```
wwwroot/
└── uploads/
    ├── banners/      (Banner images)
    ├── brands/       (Brand logos with names)
    └── partners/     (Partner logos with names)
```

### Banner Seeding Logic

- Reads all images from `wwwroot/uploads/banners/`
- Creates one Banner record per image
- Stores relative path: `/uploads/banners/{filename}`
- Skips if record already exists

**Example:**
```
banner1.jpg → Banner { ImageUrl: "/uploads/banners/banner1.jpg" }
banner2.png → Banner { ImageUrl: "/uploads/banners/banner2.png" }
```

### Brand Seeding Logic

- Reads all images from `wwwroot/uploads/brands/`
- Extracts name from filename (without extension)
- Detects if name is Arabic or English
- Translates to the other language
- Creates Brand record with both names

**Example 1 - Arabic Filename:**
```
سامسونج.png
↓
Brand {
    NameAr: "سامسونج",
    NameEn: "Samsung",
    ImageUrl: "/uploads/brands/سامسونج.png"
}
```

**Example 2 - English Filename:**
```
Apple.jpg
↓
Brand {
    NameAr: "أبل",
    NameEn: "Apple",
    ImageUrl: "/uploads/brands/Apple.jpg"
}
```

### Partner Seeding Logic

Same as Brand seeding - reads images, detects language, translates, and creates records.

## Translation Dictionary

The seeder includes a built-in translation dictionary for common brands/partners:

### Arabic → English
- سامسونج → Samsung
- أبل → Apple
- هواوي → Huawei
- شاومي → Xiaomi
- نايكي → Nike
- أديداس → Adidas
- *(and 30+ more)*

### English → Arabic
- Samsung → سامسونج
- Apple → أبل
- Nike → نايكي
- Adidas → أديداس
- *(and 30+ more)*

**Note:** If a name is not in the dictionary, the seeder will keep the original name for both languages.

## Files Created

### Infrastructure Layer

```
Infrastructure/
├── DataSeeding/
│   ├── IDataSeeder.cs                 (Interface)
│   ├── DatabaseSeeder.cs              (Main seeder implementation)
│   └── Helpers/
│       └── LanguageHelper.cs          (Translation logic)
```

### Updated Files

- `Infrastructure/DI/ServiceContainer.cs` - Registered IDataSeeder
- `Web.Api/Program.cs` - Added seeding on startup

## Usage

### Automatic Seeding

The seeder runs automatically when the application starts:

```csharp
// In Program.cs
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
    await seeder.SeedAsync();
}
```

### Manual Seeding (Optional)

You can also trigger seeding manually via dependency injection:

```csharp
public class YourController : ControllerBase
{
    private readonly IDataSeeder _seeder;

    public YourController(IDataSeeder seeder)
    {
        _seeder = seeder;
    }

    [HttpPost("seed")]
    public async Task<IActionResult> SeedDatabase()
    {
        await _seeder.SeedAsync();
        return Ok("Seeding completed");
    }
}
```

## Logging

The seeder provides detailed logging:

```
Starting database seeding...
Found 5 banner images
Added banner: /uploads/banners/banner1.jpg
Banner already exists: /uploads/banners/banner2.jpg
Banner seeding completed
Found 10 brand images
Added brand: Samsung (سامسونج) - /uploads/brands/Samsung - سامسونج.jpg
Brand seeding completed
Found 8 partner images
Added partner: Nike (نايكي) - /uploads/partners/Nike - نايكي.png
Partner seeding completed
Database seeding completed successfully.
```

## Duplicate Prevention

The seeder checks for existing records before inserting:

```csharp
var exists = await _context.Banners
    .AnyAsync(b => b.ImageUrl == relativePath && !b.IsDeleted);

if (!exists)
{
    // Add new record
}
```

This ensures:
- No duplicate records
- Safe to run multiple times
- Only new images are added

## Supported Image Formats

- `.jpg`
- `.jpeg`
- `.png`
- `.webp`

All other file types are ignored.

## Testing the Seeding

### 1. Prepare Your Images

Place images in the appropriate folders:

```bash
# Windows
mkdir Web.Api\wwwroot\uploads\banners
mkdir Web.Api\wwwroot\uploads\brands
mkdir Web.Api\wwwroot\uploads\partners

# Copy your images to these folders
```

### 2. Run the Application

```bash
dotnet run --project Web.Api
```

### 3. Check Logs

Look for seeding logs in the console output:

```
info: Infrastructure.DataSeeding.DatabaseSeeder[0]
      Starting database seeding...
info: Infrastructure.DataSeeding.DatabaseSeeder[0]
      Found 5 banner images
```

### 4. Verify Database

Check your database to confirm records were created:

```sql
SELECT * FROM banners;
SELECT * FROM brands;
SELECT * FROM partners;
```

## Adding New Brands/Partners

To add new brands or partners:

1. **Prepare the image** with bilingual filename:
   ```
   BrandName - الاسم_بالعربي.png
   ```

2. **Place in correct folder:**
   - Brands: `wwwroot/uploads/brands/`
   - Partners: `wwwroot/uploads/partners/`

3. **Restart the application** - the seeder runs automatically on startup

**Example:**
```bash
# Add a new brand
copy "Zara - زارا.png" "Web.Api\wwwroot\uploads\brands\"

# Run the app
dotnet run --project Web.Api
```

The seeder will automatically:
- Detect the new file
- Parse the English name (Zara)
- Parse the Arabic name (زارا)
- Create a database record with both names

## Extending the Seeder

### Add Custom Translation API

Replace the dictionary-based translation with an API:

```csharp
public static async Task<string> TranslateArabicToEnglish(string arabicName)
{
    // Call translation API
    var result = await translationService.TranslateAsync(arabicName, "ar", "en");
    return result;
}
```

### Add More Entities

To seed additional entities, add new methods to `DatabaseSeeder.cs`:

```csharp
private async Task SeedYourEntityAsync()
{
    var folder = Path.Combine(_wwwrootPath, "uploads", "yourentity");
    // ... similar logic
}
```

## Troubleshooting

### Images Not Being Seeded

1. **Check folder exists:**
   ```
   Web.Api\wwwroot\uploads\banners
   ```

2. **Check file extensions:**
   Only `.jpg`, `.jpeg`, `.png`, `.webp` are supported

3. **Check logs:**
   Look for warnings/errors in console output

4. **Check database:**
   Ensure records don't already exist

### Translation Not Working

Names are now parsed directly from the filename using the format `EnglishName - ArabicName`.
- Ensure your filenames follow the exact format
- The separator must be ` - ` (space-dash-space)
- Both names must be present

### Invalid Filename Format Warning

If you see warnings like:
```
Skipping brand with invalid filename format: Samsung.png. Expected format: 'EnglishName - ArabicName'
```

This means your filename doesn't follow the required format. Rename it to:
```
Samsung - سامسونج.png
```

### Seeder Not Running

Verify `Program.cs` has the seeding code:
```csharp
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
    await seeder.SeedAsync();
}
```

## Performance Considerations

- Seeding runs on startup - minimal performance impact
- Uses `AnyAsync()` for efficient duplicate checking
- Only processes image files (filters by extension)
- Logs are structured for easy monitoring

## Security Considerations

- Reads only from designated folders
- Validates file extensions
- Uses parameterized queries (EF Core)
- No user input required

## Future Enhancements

1. **Bulk Operations**
   - Use `AddRangeAsync()` for better performance
   - Batch save operations

2. **Image Validation**
   - Verify image dimensions
   - Check file size limits
   - Validate image content

3. **Configuration**
   - Make folder paths configurable
   - Add settings for custom separators

---

## Summary

✅ **Zero Configuration Required** - Works out of the box
✅ **Smart Translation** - Handles Arabic/English automatically
✅ **Production Ready** - Idempotent, logged, error-handled
✅ **Clean Architecture** - Follows project patterns
✅ **Extensible** - Easy to add translations or new entities

**The seeding runs automatically on every application startup!**
