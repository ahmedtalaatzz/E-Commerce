using Application.DTO.Banner;
using Application.Service_Contract;
using Domain.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers
{
    /// <summary>
    /// Controller for managing banners
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BannerController : ControllerBase
    {
        private readonly IBannerService _bannerService;

        public BannerController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        /// <summary>
        /// Get all active banners
        /// </summary>
        /// <returns>List of banners</returns>
        /// <response code="200">Returns the list of banners</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BannerResponseDto>),StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBanners()
        {
            var banners = await _bannerService.GetAllBannersAsync();
            return Ok(banners);
        }

        /// <summary>
        /// Get banner by ID
        /// </summary>
        /// <param name="id">Banner ID</param>
        /// <returns>Banner details</returns>
        /// <response code="200">Returns the banner</response>
        /// <response code="404">Banner not found</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(BannerResponseDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBannerById(Guid id)
        {
            var banner = await _bannerService.GetBannerByIdAsync(id);
            if (banner == null)
                return NotFound(new { message = $"Banner with ID {id} not found" });

            return Ok(banner);
        }

        /// <summary>
        /// Create a new banner
        /// </summary>
        /// <param name="dto">Banner creation data</param>
        /// <returns>Created banner</returns>
        /// <response code="201">Banner created successfully</response>
        /// <response code="400">Invalid input data</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateBanner([FromForm] AddBannerDto dto)
        {
            try
            {
                var banner = await _bannerService.CreateBannerAsync(dto);
                return CreatedAtAction(nameof(GetBannerById), new { id = banner.Id }, banner);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing banner
        /// </summary>
        /// <param name="id">Banner ID</param>
        /// <param name="dto">Banner update data</param>
        /// <returns>Updated banner</returns>
        /// <response code="200">Banner updated successfully</response>
        /// <response code="400">Invalid input data</response>
        /// <response code="404">Banner not found</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBanner(Guid id, [FromForm] UpdateBannerDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { message = "ID mismatch" });

            try
            {
                var banner = await _bannerService.UpdateBannerAsync(dto);
                return Ok(banner);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Soft delete a banner
        /// </summary>
        /// <param name="id">Banner ID</param>
        /// <returns>No content</returns>
        /// <response code="204">Banner deleted successfully</response>
        /// <response code="404">Banner not found</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBanner(Guid id)
        {
            try
            {
                await _bannerService.DeleteBannerAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get paginated banners with filtering but without sorting (sorting is not required for banners) also no search
        /// </summary>
        /// <param name="request">Pagination and filter parameters</param>
        /// <returns>Paged list of banners</returns>
        /// <response code="200">Returns paginated banners</response>
        [HttpPost("paged")]
        [ProducesResponseType(typeof(PagedResult<BannerResponseDto>),StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPagedBanners([FromBody] PagedRequest request)
        {
            var pagedBanners = await _bannerService.GetPagedBannersAsync(request);
            return Ok(pagedBanners);
        }

        /// <summary>
        /// Toggle banner active status
        /// </summary>
        /// <param name="id">Banner ID</param>
        /// <returns>Updated banner</returns>
        /// <response code="200">Status toggled successfully</response>
        /// <response code="404">Banner not found</response>
        [HttpPut("{id:guid}/toggle-status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ToggleStatus(Guid id)
        {
            try
            {
                var banner = await _bannerService.ToggleStatusAsync(id);
                return Ok(banner);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
