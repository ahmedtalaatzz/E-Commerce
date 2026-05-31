using Application.DTO.Partner;
using Domain.Shared;

namespace Application.Service_Contract
{
    /// <summary>
    /// Service interface for Partner business logic
    /// </summary>
    public interface IPartnerService
    {
        Task<IEnumerable<PartnerResponseDto>> GetAllPartnersAsync();
        Task<PartnerResponseDto?> GetPartnerByIdAsync(Guid id);
        Task<PartnerResponseDto> CreatePartnerAsync(AddPartnerDto dto);
        Task<PartnerResponseDto> UpdatePartnerAsync(UpdatePartnerDto dto);
        Task DeletePartnerAsync(Guid id);
        Task<PagedResult<PartnerResponseDto>> GetPagedPartnersAsync(PagedRequest request);
        Task<PartnerResponseDto> ToggleStatusAsync(Guid id);
    }
}
