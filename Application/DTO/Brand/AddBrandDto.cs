using Microsoft.AspNetCore.Http;

namespace e_commerce.core.DTO.Brand
{
    /// <summary>
    /// DTO for creating a new brand
    /// </summary>
    public class AddBrandDto
    {
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public IFormFile Image { get; set; } = null!;
    }
}
