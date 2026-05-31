using Application.DTO.Banner;
using Domain.Shared;

namespace Application.Service_Contract
{
    /// <summary>
    /// Service interface for Banner business logic
    /// </summary>
    public interface IBannerService
    {
        Task<IEnumerable<BannerResponseDto>> GetAllBannersAsync();
        Task<BannerResponseDto?> GetBannerByIdAsync(Guid id);
        Task<BannerResponseDto> CreateBannerAsync(AddBannerDto dto);
        Task<BannerResponseDto> UpdateBannerAsync(UpdateBannerDto dto);
        Task DeleteBannerAsync(Guid id);
        Task<PagedResult<BannerResponseDto>> GetPagedBannersAsync(PagedRequest request);
        Task<BannerResponseDto> ToggleStatusAsync(Guid id);
    }
}
