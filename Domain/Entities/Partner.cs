using Booking.Domain.Common.Base;

namespace e_commerce.core.Domain.Entities
{
    /// <summary>
    /// Partner entity for displaying partner logos
    /// </summary>
    public class Partner : SoftDeletableEntity
    {
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
