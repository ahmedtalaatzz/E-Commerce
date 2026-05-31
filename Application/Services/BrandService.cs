using Application.Service_Contract;
using AutoMapper;
using Domain.IRepositries;
using Domain.Shared;
using e_commerce.core.Domain.Entities;
using e_commerce.core.DTO.Brand;
using e_commerce.core.ServiceContracts;

namespace Application.Services
{
    /// <summary>
    /// Service implementation for Brand business logic
    /// </summary>
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public BrandService(IBrandRepository brandRepository, IFileService fileService, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BrandResponceDto>> GetAllBrandsAsync()
        {
            var brands = await _brandRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<BrandResponceDto>>(brands);
        }

        public async Task<BrandResponceDto?> GetBrandByIdAsync(Guid id)
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            return brand == null ? null : _mapper.Map<BrandResponceDto>(brand);
        }

        public async Task<BrandResponceDto> CreateBrandAsync(AddBrandDto dto)
        {
            var imageUrl = await _fileService.UploadFileAsync(dto.Image, "brands");

            if (string.IsNullOrEmpty(imageUrl))
                throw new InvalidOperationException("Failed to upload image");

            var brand = new Brand
            {
                NameAr = dto.NameAr,
                NameEn = dto.NameEn,
                ImageUrl = imageUrl,
                IsActive = true
            };

            await _brandRepository.AddAsync(brand);
            return _mapper.Map<BrandResponceDto>(brand);
        }

        public async Task<BrandResponceDto> UpdateBrandAsync(UpdateBrandDto dto)
        {
            var brand = await _brandRepository.GetByIdAsync(dto.Id);
            if (brand == null)
                throw new KeyNotFoundException($"Brand with ID {dto.Id} not found");

            brand.NameAr = dto.NameAr;
            brand.NameEn = dto.NameEn;
            brand.ModifiedAt = DateTimeOffset.UtcNow;

            if (dto.Image != null)
            {
                await _fileService.DeleteFileAsync(brand.ImageUrl);
                var imageUrl = await _fileService.UploadFileAsync(dto.Image, "brands");
                if (!string.IsNullOrEmpty(imageUrl))
                    brand.ImageUrl = imageUrl;
            }

            await _brandRepository.UpdateAsync(brand);
            return _mapper.Map<BrandResponceDto>(brand);
        }

        public async Task DeleteBrandAsync(Guid id)
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            if (brand == null)
                throw new KeyNotFoundException($"Brand with ID {id} not found");

            await _fileService.DeleteFileAsync(brand.ImageUrl);

            brand.IsDeleted = true;
            brand.DeletedAt = DateTimeOffset.UtcNow;

            await _brandRepository.UpdateAsync(brand);
        }

        public async Task<PagedResult<BrandResponceDto>> GetPagedBrandsAsync(PagedRequest request)
        {
            var pagedResult = await _brandRepository.GetPagedAsync(request);

            return new PagedResult<BrandResponceDto>
            {
                Items = _mapper.Map<List<BrandResponceDto>>(pagedResult.Items),
                TotalCount = pagedResult.TotalCount,
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize
            };
        }

        public async Task<BrandResponceDto> ToggleStatusAsync(Guid id)
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            if (brand == null)
                throw new KeyNotFoundException($"Brand with ID {id} not found");

            brand.IsActive = !brand.IsActive;
            brand.ModifiedAt = DateTimeOffset.UtcNow;

            await _brandRepository.UpdateAsync(brand);
            return _mapper.Map<BrandResponceDto>(brand);
        }
    }
}
