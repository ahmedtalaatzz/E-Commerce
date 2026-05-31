using Domain.Shared;
using e_commerce.core.Domain.Entities;

namespace Domain.IRepositries
{
    /// <summary>
    /// Repository interface for Partner entity operations
    /// </summary>
    public interface IPartnerRepository
    {
        Task<IEnumerable<Partner>> GetAllAsync();
        Task<Partner?> GetByIdAsync(Guid id);
        Task AddAsync(Partner partner);
        Task UpdateAsync(Partner partner);
        Task<PagedResult<Partner>> GetPagedAsync(PagedRequest request);
    }
}
