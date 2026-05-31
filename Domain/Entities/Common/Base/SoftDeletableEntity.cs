using Booking.Domain.Common.Interfaces;

namespace Booking.Domain.Common.Base
{
    public abstract class SoftDeletableEntity : AuditableEntity, ISoftDeletable
    {
        public bool IsDeleted { get; set; } = false;
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
