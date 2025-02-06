namespace Shared.DDD
{
    internal interface IEntity
    {
        public Int64 Id { get; set; }

        public DateTimeOffset CreateDate { get; set; }

        public string? CreateBy { get; set; }

        public DateTimeOffset UpdateDate { get; set; }

        public string? UpdateBy { get; set; }

        public DateTimeOffset? DeleteDate { get; set; }

        public string? DeleteBy { get; set; }
    }
}