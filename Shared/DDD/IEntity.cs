using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DDD
{
    internal interface IEntity
    {
        
        public Int64 Id { get; set; }

        
        public string? CreatedBy { get; set; }

        
        public DateTimeOffset CreatedDate { get; set; }

        
        public string? UpdatedBy { get; set; }

        
        public DateTimeOffset UpdatedDate { get; set; }

        
        public string? DeletedBy { get; set; }

        
        public DateTimeOffset? DeletedDate { get; set; }

    }
}