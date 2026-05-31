using Booking.Domain.Common.Base;

namespace Domain.Entities
{
    /// <summary>
    /// Banner entity for displaying promotional images
    /// </summary>
    public class Banner : SoftDeletableEntity
    {
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
