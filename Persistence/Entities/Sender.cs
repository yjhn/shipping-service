using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shipping_service.Persistence.Entities
{
    public class Sender
    {
        public ulong Id { get; set; }
        public byte[] HashedPassword { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public string Username { get; set; }
        public ICollection<Package> Packages { get; set; }
    }
}
