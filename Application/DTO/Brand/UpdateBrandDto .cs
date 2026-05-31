using Microsoft.AspNetCore.Http;

namespace e_commerce.core.DTO.Brand
{
    /// <summary>
    /// DTO for updating an existing brand
    /// </summary>
    public class UpdateBrandDto
    {
        public Guid Id { get; set; }
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }
    }
}
