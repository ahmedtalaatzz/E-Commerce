using Domain.IRepositries;
using Domain.Shared;
using e_commerce.core.Domain.Entities;
using e_commerce.infrastructure.DbContxt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositries
{
    /// <summary>
    /// Repository implementation for Brand entity
    /// </summary>
    public class BrandRepository : IBrandRepository
    {
        private readonly ApplicationDbContexts _db;

        public BrandRepository(ApplicationDbContexts db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Brand>> GetAllAsync()
        {
            return await _db.Brands
                .Where(b => !b.IsDeleted && b.IsActive)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task<Brand?> GetByIdAsync(Guid id)
        {
            return await _db.Brands
                .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
        }

        public async Task AddAsync(Brand brand)
        {
            await _db.Brands.AddAsync(brand);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Brand brand)
        {
            _db.Brands.Update(brand);
            await _db.SaveChangesAsync();
        }

        public async Task<PagedResult<Brand>> GetPagedAsync(PagedRequest request)
        {
            var query = _db.Brands.Where(b => !b.IsDeleted);

            if (request.IsActive.HasValue)
                query = query.Where(b => b.IsActive == request.IsActive.Value);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.ToLower();
                query = query.Where(b => b.NameAr.ToLower().Contains(search) || 
                                        b.NameEn.ToLower().Contains(search));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(b => b.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return new PagedResult<Brand>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}
