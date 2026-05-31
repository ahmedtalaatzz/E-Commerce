using Domain.Entities;
using Domain.IRepositries;
using Domain.Shared;
using e_commerce.infrastructure.DbContxt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositries
{
    /// <summary>
    /// Repository implementation for Banner entity
    /// </summary>
    public class BannerRepository : IBannerRepository
    {
        private readonly ApplicationDbContexts _db;

        public BannerRepository(ApplicationDbContexts db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Banner>> GetAllAsync()
        {
            return await _db.Banners
                .Where(b => !b.IsDeleted && b.IsActive)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task<Banner?> GetByIdAsync(Guid id)
        {
            return await _db.Banners
                .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
        }

        public async Task AddAsync(Banner banner)
        {
            await _db.Banners.AddAsync(banner);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Banner banner)
        {
            _db.Banners.Update(banner);
            await _db.SaveChangesAsync();
        }

        public async Task<PagedResult<Banner>> GetPagedAsync(PagedRequest request)
        {
            var query = _db.Banners.Where(b => !b.IsDeleted);

            if (request.IsActive.HasValue)
                query = query.Where(b => b.IsActive == request.IsActive.Value);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(b => b.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return new PagedResult<Banner>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}
