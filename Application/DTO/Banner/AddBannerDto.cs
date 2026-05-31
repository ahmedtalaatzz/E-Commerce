using Microsoft.AspNetCore.Http;

namespace Application.DTO.Banner
{
    /// <summary>
    /// DTO for creating a new banner
    /// </summary>
    public class AddBannerDto
    {
        public IFormFile Image { get; set; } = null!;
    }
}
