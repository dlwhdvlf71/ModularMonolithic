using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DDD
{
    public abstract class Entity : IEntity
    {
        [Column("id")]
        public Int64 Id { get; set; }

        [Column("created_by")]
        public string? CreatedBy { get; set; }

        [Column("created_date")]
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

        [Column("updated_by")]
        public string? UpdatedBy { get; set; }

        [Column("updated_date")]
        public DateTimeOffset UpdatedDate { get; set; } = DateTimeOffset.UtcNow;

        [Column("deleted_by")]
        public string? DeletedBy { get; set; }

        [Column("deleted_date")]
        public DateTimeOffset? DeletedDate { get; set; }

    }
}