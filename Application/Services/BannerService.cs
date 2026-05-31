using Application.DTO.Banner;
using Application.Service_Contract;
using AutoMapper;
using Domain.Entities;
using Domain.IRepositries;
using Domain.Shared;
using e_commerce.core.ServiceContracts;

namespace Application.Services
{
    /// <summary>
    /// Service implementation for Banner business logic
    /// </summary>
    public class BannerService : IBannerService
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public BannerService(IBannerRepository bannerRepository, IFileService fileService, IMapper mapper)
        {
            _bannerRepository = bannerRepository;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BannerResponseDto>> GetAllBannersAsync()
        {
            var banners = await _bannerRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<BannerResponseDto>>(banners);
        }

        public async Task<BannerResponseDto?> GetBannerByIdAsync(Guid id)
        {
            var banner = await _bannerRepository.GetByIdAsync(id);
            return banner == null ? null : _mapper.Map<BannerResponseDto>(banner);
        }

        public async Task<BannerResponseDto> CreateBannerAsync(AddBannerDto dto)
        {
            var imageUrl = await _fileService.UploadFileAsync(dto.Image, "banners");

            if (string.IsNullOrEmpty(imageUrl))
                throw new InvalidOperationException("Failed to upload image");

            var banner = new Banner
            {
                ImageUrl = imageUrl,
                IsActive = true
            };

            await _bannerRepository.AddAsync(banner);
            return _mapper.Map<BannerResponseDto>(banner);
        }

        public async Task<BannerResponseDto> UpdateBannerAsync(UpdateBannerDto dto)
        {
            var banner = await _bannerRepository.GetByIdAsync(dto.Id);
            if (banner == null)
                throw new KeyNotFoundException($"Banner with ID {dto.Id} not found");

            banner.ModifiedAt = DateTimeOffset.UtcNow;

            if (dto.Image != null)
            {
                await _fileService.DeleteFileAsync(banner.ImageUrl);
                var imageUrl = await _fileService.UploadFileAsync(dto.Image, "banners");
                if (!string.IsNullOrEmpty(imageUrl))
                    banner.ImageUrl = imageUrl;
            }

            await _bannerRepository.UpdateAsync(banner);
            return _mapper.Map<BannerResponseDto>(banner);
        }

        public async Task DeleteBannerAsync(Guid id)
        {
            var banner = await _bannerRepository.GetByIdAsync(id);
            if (banner == null)
                throw new KeyNotFoundException($"Banner with ID {id} not found");

            await _fileService.DeleteFileAsync(banner.ImageUrl);

            banner.IsDeleted = true;
            banner.DeletedAt = DateTimeOffset.UtcNow;

            await _bannerRepository.UpdateAsync(banner);
        }

        public async Task<PagedResult<BannerResponseDto>> GetPagedBannersAsync(PagedRequest request)
        {
            var pagedResult = await _bannerRepository.GetPagedAsync(request);

            return new PagedResult<BannerResponseDto>
            {
                Items = _mapper.Map<List<BannerResponseDto>>(pagedResult.Items),
                TotalCount = pagedResult.TotalCount,
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize
            };
        }

        public async Task<BannerResponseDto> ToggleStatusAsync(Guid id)
        {
            var banner = await _bannerRepository.GetByIdAsync(id);
            if (banner == null)
                throw new KeyNotFoundException($"Banner with ID {id} not found");

            banner.IsActive = !banner.IsActive;
            banner.ModifiedAt = DateTimeOffset.UtcNow;

            await _bannerRepository.UpdateAsync(banner);
            return _mapper.Map<BannerResponseDto>(banner);
        }
    }
}
