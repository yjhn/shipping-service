namespace shipping_service.Persistence.Entities
{
    public class Courier : IBaseEntity
    {
        public string Username { get; set; }
        public byte[] HashedPassword { get; set; }
        public ICollection<Shipment> CurrentShipments { get; set; }
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
