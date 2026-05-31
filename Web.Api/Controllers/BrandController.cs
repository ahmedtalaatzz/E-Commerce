using Application.Service_Contract;
using Domain.Shared;
using e_commerce.core.DTO.Brand;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers
{
    /// <summary>
    /// Controller for managing brands
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        /// <summary>
        /// Get all active brands
        /// </summary>
        /// <returns>List of brands</returns>
        /// <response code="200">Returns the list of brands</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BrandResponceDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBrands()
        {
            var brands = await _brandService.GetAllBrandsAsync();
            return Ok(brands);
        }

        /// <summary>
        /// Get brand by ID
        /// </summary>
        /// <param name="id">Brand ID</param>
        /// <returns>Brand details</returns>
        /// <response code="200">Returns the brand</response>
        /// <response code="404">Brand not found</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(BrandResponceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBrandById(Guid id)
        {
            var brand = await _brandService.GetBrandByIdAsync(id);
            if (brand == null)
                return NotFound(new { message = $"Brand with ID {id} not found" });

            return Ok(brand);
        }

        /// <summary>
        /// Create a new brand
        /// </summary>
        /// <param name="dto">Brand creation data</param>
        /// <returns>Created brand</returns>
        /// <response code="201">Brand created successfully</response>
        /// <response code="400">Invalid input data</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateBrand([FromForm] AddBrandDto dto)
        {
            try
            {
                var brand = await _brandService.CreateBrandAsync(dto);
                return CreatedAtAction(nameof(GetBrandById), new { id = brand.Id }, brand);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing brand
        /// </summary>
        /// <param name="id">Brand ID</param>
        /// <param name="dto">Brand update data</param>
        /// <returns>Updated brand</returns>
        /// <response code="200">Brand updated successfully</response>
        /// <response code="400">Invalid input data</response>
        /// <response code="404">Brand not found</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBrand(Guid id, [FromForm] UpdateBrandDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { message = "ID mismatch" });

            try
            {
                var brand = await _brandService.UpdateBrandAsync(dto);
                return Ok(brand);
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
        /// Soft delete a brand
        /// </summary>
        /// <param name="id">Brand ID</param>
        /// <returns>No content</returns>
        /// <response code="204">Brand deleted successfully</response>
        /// <response code="404">Brand not found</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBrand(Guid id)
        {
            try
            {
                await _brandService.DeleteBrandAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get paginated brands with filtering
        /// </summary>
        /// <param name="request">Pagination and filter parameters</param>
        /// <returns>Paged list of brands</returns>
        /// <response code="200">Returns paginated brands</response>
        [HttpPost("paged")]
        [ProducesResponseType(typeof(PagedResult<BrandResponceDto>),StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPagedBrands([FromBody] PagedRequest request)
        {
            var pagedBrands = await _brandService.GetPagedBrandsAsync(request);
            return Ok(pagedBrands);
        }

        /// <summary>
        /// Toggle brand active status
        /// </summary>
        /// <param name="id">Brand ID</param>
        /// <returns>Updated brand</returns>
        /// <response code="200">Status toggled successfully</response>
        /// <response code="404">Brand not found</response>
        [HttpPut("{id:guid}/toggle-status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ToggleStatus(Guid id)
        {
            try
            {
                var brand = await _brandService.ToggleStatusAsync(id);
                return Ok(brand);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
