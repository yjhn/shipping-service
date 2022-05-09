using System.Security.Cryptography;
using System.Text;

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
        public int? DestPmReceiverUnlockCode { get; set; }
        public int? DestPmCourierUnlockCode { get; set; }
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        // TODO: move the below methods to use cases
        public string GenerateIdHash()
        {
            return ComputeSha256Hash(Id);
        }

        public bool IsValidIdHash(string hash)
        {
            return hash == GenerateIdHash();
        }

        private static string ComputeSha256Hash(long rawData)
        {
            using SHA256 sha256Hash = SHA256.Create();
            byte[] bytes = sha256Hash.ComputeHash(BitConverter.GetBytes(rawData));

            // Convert byte array to a string
            StringBuilder builder = new();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }

            return builder.ToString();
        }
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
