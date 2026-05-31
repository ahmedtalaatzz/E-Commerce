using Domain.Shared;
using e_commerce.core.DTO.Brand;

namespace Application.Service_Contract
{
    /// <summary>
    /// Service interface for Brand business logic
    /// </summary>
    public interface IBrandService
    {
        Task<IEnumerable<BrandResponceDto>> GetAllBrandsAsync();
        Task<BrandResponceDto?> GetBrandByIdAsync(Guid id);
        Task<BrandResponceDto> CreateBrandAsync(AddBrandDto dto);
        Task<BrandResponceDto> UpdateBrandAsync(UpdateBrandDto dto);
        Task DeleteBrandAsync(Guid id);
        Task<PagedResult<BrandResponceDto>> GetPagedBrandsAsync(PagedRequest request);
        Task<BrandResponceDto> ToggleStatusAsync(Guid id);
    }
}
