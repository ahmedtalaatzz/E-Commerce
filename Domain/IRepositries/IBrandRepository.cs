using Domain.Shared;
using e_commerce.core.Domain.Entities;

namespace Domain.IRepositries
{
    /// <summary>
    /// Repository interface for Brand entity operations
    /// </summary>
    public interface IBrandRepository
    {
        Task<IEnumerable<Brand>> GetAllAsync();
        Task<Brand?> GetByIdAsync(Guid id);
        Task AddAsync(Brand brand);
        Task UpdateAsync(Brand brand);
        Task<PagedResult<Brand>> GetPagedAsync(PagedRequest request);
    }
}
