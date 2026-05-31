using Application.DTO.Partner;
using Application.Service_Contract;
using AutoMapper;
using Domain.IRepositries;
using Domain.Shared;
using e_commerce.core.Domain.Entities;
using e_commerce.core.ServiceContracts;

namespace Application.Services
{
    /// <summary>
    /// Service implementation for Partner business logic
    /// </summary>
    public class PartnerService : IPartnerService
    {
        private readonly IPartnerRepository _partnerRepository;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public PartnerService(IPartnerRepository partnerRepository, IFileService fileService, IMapper mapper)
        {
            _partnerRepository = partnerRepository;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PartnerResponseDto>> GetAllPartnersAsync()
        {
            var partners = await _partnerRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PartnerResponseDto>>(partners);
        }

        public async Task<PartnerResponseDto?> GetPartnerByIdAsync(Guid id)
        {
            var partner = await _partnerRepository.GetByIdAsync(id);
            return partner == null ? null : _mapper.Map<PartnerResponseDto>(partner);
        }

        public async Task<PartnerResponseDto> CreatePartnerAsync(AddPartnerDto dto)
        {
            var imageUrl = await _fileService.UploadFileAsync(dto.Image, "partners");

            if (string.IsNullOrEmpty(imageUrl))
                throw new InvalidOperationException("Failed to upload image");

            var partner = new Partner
            {
                NameAr = dto.NameAr,
                NameEn = dto.NameEn,
                ImageUrl = imageUrl,
                IsActive = true
            };

            await _partnerRepository.AddAsync(partner);
            return _mapper.Map<PartnerResponseDto>(partner);
        }

        public async Task<PartnerResponseDto> UpdatePartnerAsync(UpdatePartnerDto dto)
        {
            var partner = await _partnerRepository.GetByIdAsync(dto.Id);
            if (partner == null)
                throw new KeyNotFoundException($"Partner with ID {dto.Id} not found");

            partner.NameAr = dto.NameAr;
            partner.NameEn = dto.NameEn;
            partner.ModifiedAt = DateTimeOffset.UtcNow;

            if (dto.Image != null)
            {
                await _fileService.DeleteFileAsync(partner.ImageUrl);
                var imageUrl = await _fileService.UploadFileAsync(dto.Image, "partners");
                if (!string.IsNullOrEmpty(imageUrl))
                    partner.ImageUrl = imageUrl;
            }

            await _partnerRepository.UpdateAsync(partner);
            return _mapper.Map<PartnerResponseDto>(partner);
        }

        public async Task DeletePartnerAsync(Guid id)
        {
            var partner = await _partnerRepository.GetByIdAsync(id);
            if (partner == null)
                throw new KeyNotFoundException($"Partner with ID {id} not found");

            await _fileService.DeleteFileAsync(partner.ImageUrl);

            partner.IsDeleted = true;
            partner.DeletedAt = DateTimeOffset.UtcNow;

            await _partnerRepository.UpdateAsync(partner);
        }

        public async Task<PagedResult<PartnerResponseDto>> GetPagedPartnersAsync(PagedRequest request)
        {
            var pagedResult = await _partnerRepository.GetPagedAsync(request);

            return new PagedResult<PartnerResponseDto>
            {
                Items = _mapper.Map<List<PartnerResponseDto>>(pagedResult.Items),
                TotalCount = pagedResult.TotalCount,
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize
            };
        }

        public async Task<PartnerResponseDto> ToggleStatusAsync(Guid id)
        {
            var partner = await _partnerRepository.GetByIdAsync(id);
            if (partner == null)
                throw new KeyNotFoundException($"Partner with ID {id} not found");

            partner.IsActive = !partner.IsActive;
            partner.ModifiedAt = DateTimeOffset.UtcNow;

            await _partnerRepository.UpdateAsync(partner);
            return _mapper.Map<PartnerResponseDto>(partner);
        }
    }
}
