
namespace KBMGrpcService.Domain.Base
{
    public abstract class Auditable : IAuditable
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
