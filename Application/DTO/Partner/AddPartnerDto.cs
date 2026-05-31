using Microsoft.AspNetCore.Http;

namespace Application.DTO.Partner
{
    /// <summary>
    /// DTO for creating a new partner
    /// </summary>
    public class AddPartnerDto
    {
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public IFormFile Image { get; set; } = null!;
    }
}
