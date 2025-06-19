namespace KBMGrpcService.Entities.Base
{
    public abstract class Entity<TId> : IAuditable
    {
        public TId Id { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
