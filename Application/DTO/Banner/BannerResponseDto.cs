namespace Application.DTO.Banner
{
    /// <summary>
    /// DTO for banner response
    /// </summary>
    public class BannerResponseDto
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
