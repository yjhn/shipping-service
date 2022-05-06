namespace shipping_service.Persistence.Entities
{
    public class Shipment : IBaseEntity
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public Sender Sender { get; set; }
        public long SenderId { get; set; }
        public Courier? Courier { get; set; }
        public long? CourierId { get; set; }
        public PostMachine SourceMachine { get; set; }
        public long SourceMachineId { get; set; }
        public PostMachine DestinationMachine { get; set; }
        public long DestinationMachineId { get; set; }
        public ShipmentStatus Status { get; set; }
        public int? SrcPmSenderUnlockCode { get; set; }
        public int? SrcPmCourierUnlockCode { get; set; }
        public int? DestPmSenderUnlockCode { get; set; }
        public int? DestPmCourierUnlockCode { get; set; }
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }

    public enum ShipmentStatus
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
