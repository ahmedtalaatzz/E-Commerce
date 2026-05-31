using Microsoft.AspNetCore.Http;

namespace Application.DTO.Partner
{
    /// <summary>
    /// DTO for updating an existing partner
    /// </summary>
    public class UpdatePartnerDto
    {
        public Guid Id { get; set; }
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }
    }
}
