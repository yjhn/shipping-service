namespace shipping_service.Persistence.Entities
{
    public class Sender : IBaseEntity
    {
        public byte[] HashedPassword { get; set; }
        public string Username { get; set; }
        public ICollection<Shipment> Shipments { get; set; }
        public ulong Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}