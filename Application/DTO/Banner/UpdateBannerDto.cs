using Microsoft.AspNetCore.Http;

namespace Application.DTO.Banner
{
    /// <summary>
    /// DTO for updating an existing banner
    /// </summary>
    public class UpdateBannerDto
    {
        public Guid Id { get; set; }
        public IFormFile? Image { get; set; }
    }
}
