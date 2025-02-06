namespace Shared.DDD
{
    public abstract class Entity : IEntity
    {
        public long Id { get; set; }
        public DateTimeOffset CreateDate { get; set; } = DateTimeOffset.UtcNow;
        public string? CreateBy { get; set; }
        public DateTimeOffset UpdateDate { get; set; } = DateTimeOffset.UtcNow;
        public string? UpdateBy { get; set; }
        public DateTimeOffset? DeleteDate { get; set; }
        public string? DeleteBy { get; set; }
    }
}