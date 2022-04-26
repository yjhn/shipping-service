namespace shipping_service.Persistence.Entities
{
    public interface IBaseEntity
    {
        public ulong Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
