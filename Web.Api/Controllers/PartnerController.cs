using Application.DTO.Partner;
using Application.Service_Contract;
using Domain.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers
{
    /// <summary>
    /// Controller for managing partners
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PartnerController : ControllerBase
    {
        private readonly IPartnerService _partnerService;

        public PartnerController(IPartnerService partnerService)
        {
            _partnerService = partnerService;
        }

        /// <summary>
        /// Get all active partners
        /// </summary>
        /// <returns>List of partners</returns>
        /// <response code="200">Returns the list of partners</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PartnerResponseDto>),StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPartners()
        {
            var partners = await _partnerService.GetAllPartnersAsync();
            return Ok(partners);
        }

        /// <summary>
        /// Get partner by ID
        /// </summary>
        /// <param name="id">Partner ID</param>
        /// <returns>Partner details</returns>
        /// <response code="200">Returns the partner</response>
        /// <response code="404">Partner not found</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(PartnerResponseDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPartnerById(Guid id)
        {
            var partner = await _partnerService.GetPartnerByIdAsync(id);
            if (partner == null)
                return NotFound(new { message = $"Partner with ID {id} not found" });

            return Ok(partner);
        }

        /// <summary>
        /// Create a new partner
        /// </summary>
        /// <param name="dto">Partner creation data</param>
        /// <returns>Created partner</returns>
        /// <response code="201">Partner created successfully</response>
        /// <response code="400">Invalid input data</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePartner([FromForm] AddPartnerDto dto)
        {
            try
            {
                var partner = await _partnerService.CreatePartnerAsync(dto);
                return CreatedAtAction(nameof(GetPartnerById), new { id = partner.Id }, partner);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing partner
        /// </summary>
        /// <param name="id">Partner ID</param>
        /// <param name="dto">Partner update data</param>
        /// <returns>Updated partner</returns>
        /// <response code="200">Partner updated successfully</response>
        /// <response code="400">Invalid input data</response>
        /// <response code="404">Partner not found</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePartner(Guid id, [FromForm] UpdatePartnerDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { message = "ID mismatch" });

            try
            {
                var partner = await _partnerService.UpdatePartnerAsync(dto);
                return Ok(partner);
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
        /// Soft delete a partner
        /// </summary>
        /// <param name="id">Partner ID</param>
        /// <returns>No content</returns>
        /// <response code="204">Partner deleted successfully</response>
        /// <response code="404">Partner not found</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePartner(Guid id)
        {
            try
            {
                await _partnerService.DeletePartnerAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get paginated partners with filtering
        /// </summary>
        /// <param name="request">Pagination and filter parameters</param>
        /// <returns>Paged list of partners</returns>
        /// <response code="200">Returns paginated partners</response>
        [HttpPost("paged")]
        [ProducesResponseType(typeof(PagedResult<PartnerResponseDto>),StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPagedPartners([FromBody] PagedRequest request)
        {
            var pagedPartners = await _partnerService.GetPagedPartnersAsync(request);
            return Ok(pagedPartners);
        }

        /// <summary>
        /// Toggle partner active status
        /// </summary>
        /// <param name="id">Partner ID</param>
        /// <returns>Updated partner</returns>
        /// <response code="200">Status toggled successfully</response>
        /// <response code="404">Partner not found</response>
        [HttpPut("{id:guid}/toggle-status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ToggleStatus(Guid id)
        {
            try
            {
                var partner = await _partnerService.ToggleStatusAsync(id);
                return Ok(partner);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
