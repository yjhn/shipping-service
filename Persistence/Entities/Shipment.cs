namespace shipping_service.Persistence.Entities
{
    public class Shipment : IBaseEntity
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public Sender Sender { get; set; }
        public ulong SenderId { get; set; }
        public Courier? Courier { get; set; }
        public ulong? CourierId { get; set; }
        public PostMachine SourceMachine { get; set; }
        public ulong SourceMachineId { get; set; }
        public PostMachine DestinationMachine { get; set; }
        public ulong DestinationMachineId { get; set; }
        public PackageStatus Status { get; set; }
        public ulong Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }

    public enum PackageStatus
    {
        // when sender registers shipment to be sent
        RegisteredForSending,

        // when sender places the shipment in post machine
        InSourcePostMachine,

        // when courier picks up shipment from source machine
        Shipping,

        // when courier delivers shipment to destination machine
        InDestinationPostMachine,

        // when receiver picks up shipment from post machine
        Delivered
    }
}