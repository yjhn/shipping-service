namespace shipping_service.Persistence.Entities
{
    public class PostMachine : IBaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public ICollection<Shipment> PackagesWithThisSource { get; set; }
        public ICollection<Shipment> PackagesWithThisDestination { get; set; }
        public ulong Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}