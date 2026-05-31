using Domain.Entities;
using Domain.Shared;

namespace Domain.IRepositries
{
    /// <summary>
    /// Repository interface for Banner entity operations
    /// </summary>
    public interface IBannerRepository
    {
        Task<IEnumerable<Banner>> GetAllAsync();
        Task<Banner?> GetByIdAsync(Guid id);
        Task AddAsync(Banner banner);
        Task UpdateAsync(Banner banner);
        Task<PagedResult<Banner>> GetPagedAsync(PagedRequest request);
    }
}
