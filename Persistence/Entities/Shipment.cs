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
        public uint xmin { get; set; }

        // TODO: move the below methods to use cases
        public string GenerateIdHash()
        {
            return ComputeBase64(Id);
        }

        public bool IsValidIdHash(string hash)
        {
            return hash == GenerateIdHash();
        }

        private static string ComputeBase64(long rawData)
        {
            byte[] bytes = BitConverter.GetBytes(rawData + 1_000_000).Take(3).ToArray();
            string s = Convert.ToBase64String(bytes);
            // ToBase64String generates strings which are potentially unsafe for use in URLs
            s = s.Split('=')[0]; // Remove any trailing '='s
            s = s.Replace('+', '-'); // 62nd char of encoding
            s = s.Replace('/', '_'); // 63rd char of encoding
            return s;
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
