namespace shipping_service.Persistence.Entities
{
    public interface IBaseEntity
    {
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public uint xmin { get; set; }
    }
}
