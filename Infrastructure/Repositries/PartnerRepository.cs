using Domain.IRepositries;
using Domain.Shared;
using e_commerce.core.Domain.Entities;
using e_commerce.infrastructure.DbContxt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositries
{
    /// <summary>
    /// Repository implementation for Partner entity
    /// </summary>
    public class PartnerRepository : IPartnerRepository
    {
        private readonly ApplicationDbContexts _db;

        public PartnerRepository(ApplicationDbContexts db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Partner>> GetAllAsync()
        {
            return await _db.Partners
                .Where(p => !p.IsDeleted && p.IsActive)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<Partner?> GetByIdAsync(Guid id)
        {
            return await _db.Partners
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        public async Task AddAsync(Partner partner)
        {
            await _db.Partners.AddAsync(partner);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Partner partner)
        {
            _db.Partners.Update(partner);
            await _db.SaveChangesAsync();
        }

        public async Task<PagedResult<Partner>> GetPagedAsync(PagedRequest request)
        {
            var query = _db.Partners.Where(p => !p.IsDeleted);

            if (request.IsActive.HasValue)
                query = query.Where(p => p.IsActive == request.IsActive.Value);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.ToLower();
                query = query.Where(p => p.NameAr.ToLower().Contains(search) || 
                                        p.NameEn.ToLower().Contains(search));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return new PagedResult<Partner>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}
