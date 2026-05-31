namespace e_commerce.core.DTO.Brand
{
    /// <summary>
    /// DTO for brand response
    /// </summary>
    public class BrandResponceDto
    {
        public Guid Id { get; set; }
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
