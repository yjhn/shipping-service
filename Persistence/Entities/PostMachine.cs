namespace shipping_service.Persistence.Entities
{
    public class PostMachine : IBaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public ICollection<Shipment> ShipmentsWithThisSource { get; set; } = new List<Shipment>();
        public ICollection<Shipment> ShipmentsWithThisDestination { get; set; } = new List<Shipment>();
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public uint xmin { get; set; }
    }
}
